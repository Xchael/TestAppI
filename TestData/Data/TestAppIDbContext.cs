using System;
using System.Collections.Generic;
using Microsoft.EntityFrameworkCore;
using TestData.Models;

namespace TestData.Data;

public partial class TestAppIDbContext : DbContext
{
    public TestAppIDbContext()
    {
    }

    public TestAppIDbContext(DbContextOptions<TestAppIDbContext> options)
        : base(options)
    {
    }

    public virtual DbSet<TestTable> TestTable { get; set; }

    //protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    //    => optionsBuilder.UseSqlServer("Data Source=.;Initial Catalog=TestAppIDb;Integrated Security=True;Trust Server Certificate=True;Command Timeout=300");

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<TestTable>(entity =>
        {
            entity.Property(e => e.IsEnabled).HasDefaultValue(true);
        });

        OnModelCreatingGeneratedFunctions(modelBuilder);
        OnModelCreatingPartial(modelBuilder);
    }

    partial void OnModelCreatingPartial(ModelBuilder modelBuilder);
}
