using Microsoft.EntityFrameworkCore;
using NHSHealthPortal.Core.Entities;

namespace NHSHealthPortal.Data.DataContext;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options) : base(options)
    {
    }

    public DbSet<Patient> Patients => Set<Patient>();
    
    protected override void OnModelCreating(ModelBuilder b)
    {
        // add the key as Id
        b.Entity<Patient>().HasKey(x => x.Id);
        b.Entity<Patient>().Property(x => x.Id).ValueGeneratedOnAdd();

        // add the index on nhs number to find records quickly
        b.Entity<Patient>().HasIndex(x => x.NhsNumber).IsUnique();
    }
}