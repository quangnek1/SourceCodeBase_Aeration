using AerationSterilize.Domain.Entities;
using AerationSterilize.Domain.Entities.BoxingPositions;
using AerationSterilize.Domain.Entities.Identity;
using AerationSterilize.Domain.Entities.Working;
using Contracts.Abstractions.Entities.Domains.Interfaces;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace AerationSterilize.Persistence;

public class ApplicationDbContext : IdentityDbContext<AppUser, AppRole, Guid>
{
    public ApplicationDbContext(DbContextOptions<ApplicationDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder builder)
        => builder.ApplyConfigurationsFromAssembly(AssemblyReference.Assembly);

    public DbSet<AppUser> AppUses { get; set; }
    public DbSet<Domain.Entities.Identity.Action> Actions { get; set; }
    public DbSet<Function> Functions { get; set; }
    public DbSet<ActionInFunction> ActionInFunctions { get; set; }
    public DbSet<Permission> Permissions { get; set; }
    public DbSet<UserSession> UserSessions { get; set; }

    public DbSet<Product> Products { get; set; }
    public DbSet<AerationColumn> AerationColumns { get; set; }
    public DbSet<AerationPosition> AerationPositions { get; set; }
    public DbSet<Setting> Settings { get; set; }

    public DbSet<BoxingColumn> BoxingColumns { get; set; }
    public DbSet<BoxingPosition> BoxingPositions { get; set; }

    public DbSet<PackingColumn> PackingColumns { get; set; }
    public DbSet<PackingPosition> PackingPositions { get; set; }

    public DbSet<BoxingJob> BoxingJobs { get; set; }
    public DbSet<Box> Boxes { get; set; }
    public DbSet<ItemTag> ItemTags { get; set; }
    public DbSet<WorkLine> WorkLines { get; set; }
    public DbSet<WorkTable> WorkTables { get; set; }
    public DbSet<WorkTableSession> WorkTableSessions { get; set; }
    public DbSet<WorkTableAssignment> WorkTableAssignments { get; set; }
    public DbSet<BoxingSession> BoxingSessions { get; set; }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = new CancellationToken())
    {
        var modified = ChangeTracker.Entries()
            .Where(e => e.State == EntityState.Modified
                        || e.State == EntityState.Added
                        || e.State == EntityState.Deleted);

        foreach (var item in modified)
        {
            switch (item.State)
            {
                case EntityState.Added:
                    if (item.Entity is IDateTracking addedEntity)
                    {
                        addedEntity.CreatedDate = DateTimeOffset.Now;
                        item.State = EntityState.Added;
                    }
                    break;

                case EntityState.Modified:
                    Entry(item.Entity).Property("Id").IsModified = false;
                    if (item.Entity is IDateTracking modifiedEntity)
                    {
                        modifiedEntity.LastModifiedDate = DateTimeOffset.Now;
                        item.State = EntityState.Modified;
                    }
                    break;
            }
        }

        var result = await base.SaveChangesAsync(cancellationToken);

        return result;
    }
}
