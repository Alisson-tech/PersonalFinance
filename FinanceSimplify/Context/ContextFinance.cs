using FinanceSimplify.Data;
using Microsoft.EntityFrameworkCore;

namespace FinanceSimplify.Context;

public class ContextFinance : DbContext
{
    public ContextFinance(DbContextOptions<ContextFinance> options)
        : base(options)
    {
    }

    public DbSet<Accounts> Accounts { get; set; }
    public DbSet<Transactions> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Accounts>()
            .HasKey(a => a.Id);

        modelBuilder.Entity<Transactions>()
            .HasKey(t => t.Id);

        modelBuilder.Entity<Accounts>()
            .HasMany(a => a.Transactions)
            .WithOne(t => t.Account)
            .HasForeignKey(t => t.AccountId);

        modelBuilder.Entity<Accounts>()
            .HasQueryFilter(a => a.DateDeleted != null);

    }

    public override int SaveChanges()
    {
        ProcessEntityChanges();

        return base.SaveChanges();
    }

    public override async Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        ProcessEntityChanges();

        return await base.SaveChangesAsync(cancellationToken);
    }

    private void ProcessEntityChanges()
    {
        var entries = ChangeTracker.Entries()
            .Where(e => e.Entity is BaseEntity &&
                        (e.State == EntityState.Added || e.State == EntityState.Modified));

        foreach (var entry in entries)
        {
            var entity = (BaseEntity)entry.Entity;

            switch (entry.State)
            {
                case EntityState.Added:
                    entity.DateCreated = DateTime.UtcNow;
                    break;

                case EntityState.Modified:
                    entity.DateUpdated = DateTime.UtcNow;
                    break;

            }
        }
    }
}
