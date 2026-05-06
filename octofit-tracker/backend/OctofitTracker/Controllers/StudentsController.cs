using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using OctofitTracker.Data;
using OctofitTracker.Models;

namespace OctofitTracker.Controllers;

[ApiController]
[Route("api/[controller]")]
public class StudentsController : ControllerBase
{
    private readonly OctofitDbContext _db;

    public StudentsController(OctofitDbContext db) => _db = db;

    // GET api/students
    [HttpGet]
    public async Task<IActionResult> GetAll() =>
        Ok(await _db.Students.Select(s => new { s.Id, s.Username, s.Email, s.RegisteredAt }).ToListAsync());

    // GET api/students/5
    [HttpGet("{id}")]
    public async Task<IActionResult> Get(int id)
    {
        var student = await _db.Students.FindAsync(id);
        if (student is null) return NotFound();
        return Ok(new { student.Id, student.Username, student.Email, student.RegisteredAt });
    }

    // POST api/students/register
    [HttpPost("register")]
    public async Task<IActionResult> Register([FromBody] RegisterRequest request)
    {
        if (string.IsNullOrWhiteSpace(request.Username))
            return BadRequest(new { message = "Username is required." });

        if (string.IsNullOrWhiteSpace(request.Email) || !request.Email.Contains('@'))
            return BadRequest(new { message = "A valid email address is required." });

        if (string.IsNullOrWhiteSpace(request.Password) || request.Password.Length < 6)
            return BadRequest(new { message = "Password must be at least 6 characters." });

        if (await _db.Students.AnyAsync(s => s.Username == request.Username))
            return Conflict(new { message = "Username already taken." });

        if (await _db.Students.AnyAsync(s => s.Email == request.Email))
            return Conflict(new { message = "Email already registered." });

        var student = new Student
        {
            Username = request.Username,
            Email = request.Email,
            PasswordHash = BCrypt.Net.BCrypt.HashPassword(request.Password)
        };

        _db.Students.Add(student);
        await _db.SaveChangesAsync();

        return CreatedAtAction(nameof(Get), new { id = student.Id },
            new { student.Id, student.Username, student.Email, student.RegisteredAt });
    }
}

public record RegisterRequest(string Username, string Email, string Password);
