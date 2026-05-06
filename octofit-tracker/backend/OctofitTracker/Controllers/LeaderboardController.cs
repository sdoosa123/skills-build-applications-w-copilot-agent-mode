using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OctofitTracker.Data;
using OctofitTracker.Models;

namespace OctofitTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class LeaderboardController : ControllerBase
{
    private readonly OctofitDbContext _db;

    public LeaderboardController(OctofitDbContext db) => _db = db;

    // GET api/leaderboard
    [HttpGet]
    public async Task<IActionResult> Get()
    {
        var entries = await _db.Students
            .Select(s => new
            {
                StudentId = s.Id,
                s.Username,
                TotalPoints = s.WorkoutLogs.Sum(wl => wl.Workout.Points)
            })
            .OrderByDescending(e => e.TotalPoints)
            .ToListAsync();

        var ranked = entries
            .Select((e, i) => new LeaderboardEntry
            {
                StudentId = e.StudentId,
                Username = e.Username,
                TotalPoints = e.TotalPoints,
                Rank = i + 1
            })
            .ToList();

        return Ok(ranked);
    }
}
