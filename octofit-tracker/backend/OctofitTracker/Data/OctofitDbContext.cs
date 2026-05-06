using Microsoft.EntityFrameworkCore;
using OctofitTracker.Models;

namespace OctofitTracker.Data;

public class OctofitDbContext : DbContext
{
    public OctofitDbContext(DbContextOptions<OctofitDbContext> options) : base(options) { }

    public DbSet<Student> Students => Set<Student>();
    public DbSet<Workout> Workouts => Set<Workout>();
    public DbSet<WorkoutLog> WorkoutLogs => Set<WorkoutLog>();

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Student>()
            .HasIndex(s => s.Username)
            .IsUnique();

        modelBuilder.Entity<Student>()
            .HasIndex(s => s.Email)
            .IsUnique();

        modelBuilder.Entity<WorkoutLog>()
            .HasOne(wl => wl.Student)
            .WithMany(s => s.WorkoutLogs)
            .HasForeignKey(wl => wl.StudentId);

        modelBuilder.Entity<WorkoutLog>()
            .HasOne(wl => wl.Workout)
            .WithMany(w => w.WorkoutLogs)
            .HasForeignKey(wl => wl.WorkoutId);

        // Seed data
        modelBuilder.Entity<Workout>().HasData(
            new Workout { Id = 1, Name = "Running", Description = "Outdoor or treadmill run", Points = 10 },
            new Workout { Id = 2, Name = "Cycling", Description = "Bike ride or stationary bike", Points = 8 },
            new Workout { Id = 3, Name = "Swimming", Description = "Lap swimming", Points = 12 },
            new Workout { Id = 4, Name = "Strength Training", Description = "Weight or resistance training", Points = 9 },
            new Workout { Id = 5, Name = "Yoga", Description = "Yoga or stretching session", Points = 5 }
        );
    }
}
