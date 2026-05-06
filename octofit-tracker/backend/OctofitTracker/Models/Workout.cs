namespace OctofitTracker.Models;

public class Workout
{
    public int Id { get; set; }
    public string Name { get; set; } = string.Empty;
    public string Description { get; set; } = string.Empty;
    public int Points { get; set; }

    public ICollection<WorkoutLog> WorkoutLogs { get; set; } = new List<WorkoutLog>();
}
