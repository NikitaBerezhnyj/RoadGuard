using Microsoft.EntityFrameworkCore;
using RoadGuard.Models.Entities;

namespace RoadGuard.Data
{
  public class AppDbContext : DbContext
  {
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public DbSet<User> Users { get; set; } = null!;
    // public DbSet<DriverLocation> DriverLocations { get; set; } = null!;
    public DbSet<DriverRating> DriverRatings { get; set; } = null!;
    public DbSet<Report> Reports { get; set; } = null!;

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
      base.OnModelCreating(modelBuilder);

      modelBuilder.Entity<User>()
          .HasIndex(u => u.Username)
          .IsUnique();

      modelBuilder.Entity<DriverRating>()
          .HasOne(r => r.FromUser)
          .WithMany(u => u.RatingsGiven)
          .HasForeignKey(r => r.FromUserId)
          .OnDelete(DeleteBehavior.Restrict);

      modelBuilder.Entity<DriverRating>()
          .HasOne(r => r.ToUser)
          .WithMany(u => u.RatingsReceived)
          .HasForeignKey(r => r.ToUserId)
          .OnDelete(DeleteBehavior.Restrict);
    }
  }
}
