using System.ComponentModel.DataAnnotations;

namespace BettingSportsApi.Models;

public class Match
{
    public int Id { get; set; }
    
    [Required]
    [MaxLength(100)]
    public string HomeTeam { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(100)]
    public string AwayTeam { get; set; } = string.Empty;
    
    [Required]
    [MaxLength(50)]
    public string League { get; set; } = string.Empty; 
    
    [Range(1.0, 10.0)]
    public double HomeOdds { get; set; } = 2.0; 
    
    [Range(1.0, 10.0)]
    public double AwayOdds { get; set; } = 2.0; 
    
    [Range(1.0, 10.0)]
    public double DrawOdds { get; set; } = 2.0; 
    
    public int HomeScore { get; set; } = 0;
    public int AwayScore { get; set; } = 0;
    
    public bool IsCompleted { get; set; } = false;
    
    public DateTime MatchDate { get; set; } = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
}