using API.Features.Domain;
using Microsoft.EntityFrameworkCore;

namespace API.Features.Infrastructure.Contexts;

public class AccountContext(DbContextOptions<AccountContext> options) : DbContext(options)
{
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureAccountEntity(modelBuilder);
    }

    private void ConfigureAccountEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.ToTable("Transactions");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.Amount).IsRequired().HasColumnType("decimal(18,2)");
            entity.Property(a => a.Time).IsRequired();
        });
    }
}