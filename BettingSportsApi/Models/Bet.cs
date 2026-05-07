using System.ComponentModel.DataAnnotations;

namespace BettingSportsApi.Models;

public class Bet
{
    public int Id { get; set; }
    
    [Required]
    public int MatchId { get; set; }
    
    [Required]
    [MaxLength(20)]
    public string BetType { get; set; } = string.Empty; 
    
    [Required]
    public decimal Amount { get; set; }
    
    [Required]
    public decimal Odds { get; set; }
    
    public bool IsWin { get; set; } = false;
    
    public decimal Payout { get; set; } = 0;
    
    public DateTime BetDate { get; set; } = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
}