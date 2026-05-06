namespace OctofitTracker.Models;

public class LeaderboardEntry
{
    public int StudentId { get; set; }
    public string Username { get; set; } = string.Empty;
    public int TotalPoints { get; set; }
    public int Rank { get; set; }
}
