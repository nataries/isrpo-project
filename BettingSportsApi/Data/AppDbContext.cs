using Microsoft.EntityFrameworkCore;
using BettingSportsApi.Models;

namespace BettingSportsApi.Data;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }
    
    public DbSet<Match> Matches { get; set; }
    public DbSet<UserState> UserStates { get; set; }
    public DbSet<Bet> Bets { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        
        modelBuilder.Entity<Match>().HasData(
            new Match { Id = 1, HomeTeam = "ПСЖ", AwayTeam = "Бавария", League = "Champions League", HomeOdds = 2.1, AwayOdds = 3.2, DrawOdds = 3.5, HomeScore = 0, AwayScore = 0, IsCompleted = false },
            new Match { Id = 2, HomeTeam = "Арсенал", AwayTeam = "Атлетико Мадрид", League = "Champions League", HomeOdds = 1.8, AwayOdds = 4.0, DrawOdds = 3.6, HomeScore = 0, AwayScore = 0, IsCompleted = false },
            new Match { Id = 3, HomeTeam = "Спортинг", AwayTeam = "Арсенал", League = "Champions League", HomeOdds = 2.3, AwayOdds = 2.9, DrawOdds = 3.4, HomeScore = 0, AwayScore = 0, IsCompleted = false },
            new Match { Id = 4, HomeTeam = "Галатасарай", AwayTeam = "Ливерпуль", League = "Champions League", HomeOdds = 2.2, AwayOdds = 3.1, DrawOdds = 3.3, HomeScore = 0, AwayScore = 0, IsCompleted = false },
            new Match { Id = 5, HomeTeam = "Леванте", AwayTeam = "Осасуна", League = "La Liga", HomeOdds = 1.95, AwayOdds = 3.8, DrawOdds = 3.4, HomeScore = 0, AwayScore = 0, IsCompleted = false },
            new Match { Id = 6, HomeTeam = "Атлетико", AwayTeam = "Сельта", League = "La Liga", HomeOdds = 1.75, AwayOdds = 4.5, DrawOdds = 3.7, HomeScore = 0, AwayScore = 0, IsCompleted = false },
            new Match { Id = 7, HomeTeam = "Реал Сосьедад", AwayTeam = "Бетис", League = "La Liga", HomeOdds = 2.4, AwayOdds = 2.8, DrawOdds = 3.2, HomeScore = 0, AwayScore = 0, IsCompleted = false },
            new Match { Id = 8, HomeTeam = "Мальорка", AwayTeam = "Вильярреал", League = "La Liga", HomeOdds = 2.6, AwayOdds = 2.6, DrawOdds = 3.1, HomeScore = 0, AwayScore = 0, IsCompleted = false }
        );
        
        modelBuilder.Entity<UserState>().HasData(
            new UserState { Id = 1, Balance = 10000 }
        );
    }
}