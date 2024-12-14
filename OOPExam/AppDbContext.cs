using Microsoft.EntityFrameworkCore;
using OOPExam.Classes;

namespace OOPExam;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options) { }

    public AppDbContext()
    {
        
    }

    public DbSet<Client> Clients { get; set; }
    public DbSet<Doctor> Doctors { get; set; }
    public DbSet<Diagnosis> Diagnoses { get; set; }
    public DbSet<Visit> Visits { get; set; }
    
    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        optionsBuilder.UseSqlite("Data Source=myapp.db");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Visit>()
            .HasOne(p => p.Doctor)
            .WithMany();
        modelBuilder.Entity<Visit>()
            .HasOne(p => p.Client)
            .WithMany();
        modelBuilder.Entity<Visit>()
            .HasOne(p => p.Diagnosis).WithMany();

    }

}