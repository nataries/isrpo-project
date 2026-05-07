using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using BettingSportsApi.Data;
using BettingSportsApi.Models;

namespace BettingSportsApi.Controllers;

[ApiController]
[Route("api/[controller]")]
public class BettingController : ControllerBase
{
    private readonly AppDbContext _db;
    
    public BettingController(AppDbContext db)
    {
        _db = db;
    }
    
    [HttpGet("balance")]
    public async Task<ActionResult<decimal>> GetBalance()
    {
        var userState = await _db.UserStates.FirstOrDefaultAsync();
        if (userState == null)
        {
            userState = new UserState { Balance = 10000 };
            _db.UserStates.Add(userState);
            await _db.SaveChangesAsync();
        }
        return Ok(new { balance = userState.Balance });
    }
    
    [HttpGet("matches")]
    public async Task<ActionResult<List<Match>>> GetMatches()
    {
        var matches = await _db.Matches
            .OrderBy(m => m.IsCompleted)
            .ThenBy(m => m.MatchDate)
            .ToListAsync();
        return Ok(matches);
    }
    
    [HttpGet("matches/league/{league}")]
    public async Task<ActionResult<List<Match>>> GetMatchesByLeague(string league)
    {
        var matches = await _db.Matches
            .Where(m => m.League == league)
            .OrderBy(m => m.IsCompleted)
            .ThenBy(m => m.MatchDate)
            .ToListAsync();
        return Ok(matches);
    }
    
    [HttpPost("bet")]
    public async Task<ActionResult<Bet>> PlaceBet([FromBody] BetRequest request)
    {
        var match = await _db.Matches.FindAsync(request.MatchId);
        if (match == null)
        {
            return NotFound(new { message = "Матч не найден" });
        }
        
        if (match.IsCompleted)
        {
            return BadRequest(new { message = "Этот матч уже завершен, нельзя сделать ставку" });
        }
        
        var userState = await _db.UserStates.FirstOrDefaultAsync();
        if (userState == null)
        {
            userState = new UserState { Balance = 10000 };
            _db.UserStates.Add(userState);
            await _db.SaveChangesAsync();
        }
        
        if (userState.Balance < request.Amount)
        {
            return BadRequest(new { message = "Недостаточно средств на счету" });
        }
        
        decimal odds = (decimal)match.HomeOdds;
        
        var bet = new Bet
        {
            MatchId = request.MatchId,
            BetType = "home", 
            Amount = request.Amount,
            Odds = odds,
            BetDate = DateTime.UtcNow,
            IsWin = false, 
            Payout = 0
        };
        
        userState.Balance -= request.Amount;
        userState.LastUpdated = DateTime.UtcNow;
        
        _db.Bets.Add(bet);
        await _db.SaveChangesAsync();
        
        return Ok(new { 
            bet = new { 
                bet.Id, 
                bet.Amount, 
                bet.Odds, 
                bet.BetType,
                bet.IsWin,
                bet.Payout
            }, 
            newBalance = userState.Balance 
        });
    }
    
    [HttpPost("complete-match/{matchId}")]
    public async Task<ActionResult> CompleteMatch(int matchId, [FromBody] MatchResult result)
    {
        var match = await _db.Matches.FindAsync(matchId);
        if (match == null)
        {
            return NotFound(new { message = "Матч не найден" });
        }
        
        if (match.IsCompleted)
        {
            return BadRequest(new { message = "Матч уже завершен" });
        }
        
        match.HomeScore = result.HomeScore;
        match.AwayScore = result.AwayScore;
        match.IsCompleted = true;
        
        bool isHomeWin = match.HomeScore > match.AwayScore;
        
        var bets = await _db.Bets.Where(b => b.MatchId == matchId && !b.IsWin).ToListAsync();
        var userState = await _db.UserStates.FirstOrDefaultAsync();
        
        decimal totalPayout = 0;
        
        foreach (var bet in bets)
        {
            if (isHomeWin && bet.BetType == "home")
            {
                bet.IsWin = true;
                bet.Payout = bet.Amount * bet.Odds;
                totalPayout += bet.Payout;
            }
        }
        
        if (userState != null && totalPayout > 0)
        {
            userState.Balance += totalPayout;
            userState.LastUpdated = DateTime.UtcNow;
        }
        
        await _db.SaveChangesAsync();
        
        return Ok(new 
        { 
            match = new { match.Id, match.HomeScore, match.AwayScore, match.IsCompleted },
            isHomeWin,
            homeScore = match.HomeScore,
            awayScore = match.AwayScore,
            totalPayout,
            newBalance = userState?.Balance ?? 10000
        });
    }
    
    [HttpPost("reset-balance")]
    public async Task<ActionResult> ResetBalance()
    {
        var userState = await _db.UserStates.FirstOrDefaultAsync();
        if (userState != null)
        {
            userState.Balance = 10000;
            userState.LastUpdated = DateTime.UtcNow;
            await _db.SaveChangesAsync();
        }
        
        return Ok(new { message = "Баланс сброшен до 10 000 монет", balance = 10000 });
    }
    
    [HttpGet("my-bets")]
    public async Task<ActionResult<List<Bet>>> GetMyBets()
    {
        var bets = await _db.Bets
            .Include(b => b.MatchId)
            .OrderByDescending(b => b.BetDate)
            .ToListAsync();
        return Ok(bets);
    }
}

public class BetRequest
{
    public int MatchId { get; set; }
    public string BetType { get; set; } = string.Empty;
    public decimal Amount { get; set; }
}

public class MatchResult
{
    public int HomeScore { get; set; }
    public int AwayScore { get; set; }
}