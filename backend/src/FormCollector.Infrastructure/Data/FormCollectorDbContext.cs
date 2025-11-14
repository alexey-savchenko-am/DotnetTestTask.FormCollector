using FormCollector.Domain;
using Microsoft.EntityFrameworkCore;

namespace FormCollector.Infrastructure.Data;

internal sealed class FormCollectorDbContext
    : DbContext
{
    public DbSet<Submission> Sumbissions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);
        modelBuilder.ApplyConfigurationsFromAssembly(typeof(FormCollectorDbContext).Assembly);
    }
}
