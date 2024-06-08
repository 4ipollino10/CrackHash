using CrackHash.Manager.Core.Entities;
using Microsoft.EntityFrameworkCore;
using MongoDB.EntityFrameworkCore.Extensions;

namespace CrackHash.Manager.Infrastructure;

public class ApplicationDbContext : DbContext
{
    public DbSet<TaskRequest> TaskRequests { get; init; } = null!;

    public ApplicationDbContext(DbContextOptions options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
    {
        builder.Entity<TaskRequest>(x =>
        {
            x.ToCollection("TaskRequests");
            x.HasKey(xx => xx.Id);
            x.Property(xx => xx.Id).ValueGeneratedOnAdd();
        });
    }
}