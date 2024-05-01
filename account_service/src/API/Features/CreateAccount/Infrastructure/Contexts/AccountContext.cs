using API.Features.CreateAccount.Domain;
using Microsoft.EntityFrameworkCore;

namespace API.Features.CreateAccount.Infrastructure.Contexts;

public class AccountContext(DbContextOptions<AccountContext> options) : DbContext(options)
{
    public DbSet<Account> Accounts { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        ConfigureAccountEntity(modelBuilder);
    }

    private void ConfigureAccountEntity(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>(entity =>
        {
            entity.ToTable("Accounts");
            entity.HasKey(a => a.Id);
            entity.Property(a => a.CPR).IsRequired();
            entity.Property(a => a.FirstName).IsRequired().HasMaxLength(100);
            entity.Property(a => a.LastName).IsRequired().HasMaxLength(100);
            entity.Property(a => a.Balance).HasColumnType("decimal(18,2)");
        });
    }
}