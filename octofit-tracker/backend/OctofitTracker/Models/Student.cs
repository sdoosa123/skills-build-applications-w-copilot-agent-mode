namespace OctofitTracker.Models;

public class Student
{
    public int Id { get; set; }
    public string Username { get; set; } = string.Empty;
    public string Email { get; set; } = string.Empty;
    public string PasswordHash { get; set; } = string.Empty;
    public DateTime RegisteredAt { get; set; } = DateTime.UtcNow;

    public ICollection<WorkoutLog> WorkoutLogs { get; set; } = new List<WorkoutLog>();
}
