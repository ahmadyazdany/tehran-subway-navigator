using DsDemo.Data.Entities;
using Microsoft.EntityFrameworkCore;

namespace DsDemo.Data;

public class ApplicationDbContext : DbContext
{
    public ApplicationDbContext(DbContextOptions options) : base(options) { }

    public DbSet<Station> Stations { get; set; }
    public DbSet<Way> Ways { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Way>().HasIndex(w => new { w.From, w.To }).IsUnique();
    }
}