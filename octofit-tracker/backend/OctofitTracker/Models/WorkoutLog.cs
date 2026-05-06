namespace OctofitTracker.Models;

public class WorkoutLog
{
    public int Id { get; set; }
    public int StudentId { get; set; }
    public int WorkoutId { get; set; }
    public DateTime LoggedAt { get; set; } = DateTime.UtcNow;

    public Student Student { get; set; } = null!;
    public Workout Workout { get; set; } = null!;
}
