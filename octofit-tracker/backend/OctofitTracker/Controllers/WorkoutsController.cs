using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OctofitTracker.Data;
using OctofitTracker.Models;

namespace OctofitTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class WorkoutsController : ControllerBase
{
    private readonly OctofitDbContext _db;

    public WorkoutsController(OctofitDbContext db) => _db = db;

    // GET api/workouts
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _db.Workouts.ToListAsync());

    // GET api/workouts/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var workout = await _db.Workouts.FindAsync(id);
        if (workout is null) return NotFound();
        return Ok(workout);
    }

    // POST api/workouts/log
    [HttpPost("log")]
    public async Task<IActionResult> LogWorkout([FromBody] LogWorkoutRequest request)
    {
        if (request.StudentId <= 0)
            return BadRequest(new { message = "StudentId must be a positive integer." });

        if (request.WorkoutId <= 0)
            return BadRequest(new { message = "WorkoutId must be a positive integer." });

        var student = await _db.Students.FindAsync(request.StudentId);
        if (student is null) return NotFound(new { message = "Student not found." });

        var workout = await _db.Workouts.FindAsync(request.WorkoutId);
        if (workout is null) return NotFound(new { message = "Workout not found." });

        var log = new WorkoutLog
        {
            StudentId = request.StudentId,
            WorkoutId = request.WorkoutId,
            LoggedAt = DateTime.UtcNow
        };

        _db.WorkoutLogs.Add(log);
        await _db.SaveChangesAsync();

        return Ok(new
        {
            log.Id,
            log.StudentId,
            log.WorkoutId,
            WorkoutName = workout.Name,
            PointsEarned = workout.Points,
            log.LoggedAt
        });
    }

    // GET api/workouts/logs/{studentId}
    [HttpGet("logs/{studentId}")]
    public async Task<IActionResult> GetLogs(int studentId)
    {
        var logs = await _db.WorkoutLogs
            .Where(wl => wl.StudentId == studentId)
            .Include(wl => wl.Workout)
            .Select(wl => new
            {
                wl.Id,
                wl.WorkoutId,
                WorkoutName = wl.Workout.Name,
                Points = wl.Workout.Points,
                wl.LoggedAt
            })
            .OrderByDescending(wl => wl.LoggedAt)
            .ToListAsync();

        return Ok(logs);
    }

    // GET api/workouts/points/{studentId}
    [HttpGet("points/{studentId}")]
    public async Task<IActionResult> GetPoints(int studentId)
    {
        var student = await _db.Students.FindAsync(studentId);
        if (student is null) return NotFound(new { message = "Student not found." });

        var totalPoints = await _db.WorkoutLogs
            .Where(wl => wl.StudentId == studentId)
            .Include(wl => wl.Workout)
            .SumAsync(wl => wl.Workout.Points);

        return Ok(new { studentId, username = student.Username, totalPoints });
    }
}

public record LogWorkoutRequest(int StudentId, int WorkoutId);
