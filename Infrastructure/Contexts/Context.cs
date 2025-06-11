using System.Linq.Expressions;
using Core;
using Core.SeedWork;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure.Contexts;

public class Context : DbContext
{
    public Context(DbContextOptions<Context> options) : base(options)
    {
    }

    protected override void OnModelCreating(ModelBuilder mb)
    {
        mb.Entity<User>(e =>
        {
            e.Property(p => p.Username)
                .HasMaxLength(160)
                .IsRequired();

            e.HasIndex(i => i.Email)
                .IsUnique();
            e.Property(p => p.Email)
                .HasMaxLength(320)
                .IsRequired();

            e.Property(p => p.Password)
                .HasMaxLength(255)
                .IsRequired();

            e.Property(p => p.Role)
                .IsRequired();
        });

        SetFilterUniqueWhenDeletedAtIsNotNull(mb);
        FilterDeletedAtNullValues(mb);
        base.OnModelCreating(mb);
    }

    public DbSet<User> Users { get; set; }

    private void SetFilterUniqueWhenDeletedAtIsNotNull(ModelBuilder mb)
    {
        foreach (var type in mb.Model.GetEntityTypes())
            if (typeof(GenericModel).IsAssignableFrom(type.ClrType))
                foreach (var index in type.GetIndexes())
                    if (index.IsUnique)
                        index.SetFilter("DeletedAt IS NULL");
    }

    public override int SaveChanges()
    {
        UpdateModifiedAtAndCreatedAt();
        return base.SaveChanges();
    }

    public override Task<int> SaveChangesAsync(CancellationToken cancellationToken = default)
    {
        UpdateModifiedAtAndCreatedAt();
        return base.SaveChangesAsync(cancellationToken);
    }

    private void UpdateModifiedAtAndCreatedAt()
    {
        var entries = ChangeTracker
            .Entries()
            .Where(e => e.Entity is GenericModel);

        foreach (var entry in entries)
        {
            if (entry.State == EntityState.Added)
            {
                ((GenericModel)entry.Entity).SetCreatedAt();
                ((GenericModel)entry.Entity).SetUpdatedAt();
            }
            else if (entry.State == EntityState.Modified)
                ((GenericModel)entry.Entity).SetUpdatedAt();
        }
    }

    private void FilterDeletedAtNullValues(ModelBuilder mb)
    {
        foreach (var type in mb.Model.GetEntityTypes())
        {
            var clrType = type.ClrType;

            if (typeof(GenericModel).IsAssignableFrom(clrType))
            {
                var param = Expression.Parameter(clrType, "e");
                
                var property = Expression.Property(param, nameof(GenericModel.DeletedAt));
                var nullConstant = Expression.Constant(null, typeof(DateTime?));
                var body = Expression.Equal(property, nullConstant);

                var lambda = Expression.Lambda(body, param);

                mb.Entity(clrType).HasQueryFilter(lambda);
            }
        }
    }
}