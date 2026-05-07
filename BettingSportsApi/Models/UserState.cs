using System.ComponentModel.DataAnnotations;

namespace BettingSportsApi.Models;

public class UserState
{
    public int Id { get; set; }
    
    [Required]
    public decimal Balance { get; set; } = 10000; 
    
    public DateTime LastUpdated { get; set; } = new DateTime(2026, 1, 1, 0, 0, 0, DateTimeKind.Utc);
}