using API.Domain;
using Microsoft.EntityFrameworkCore;

namespace API.Infrastructure;

public class AccountContext : DbContext
{
    public DbSet<SavingsAccount> SavingsAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Replace 'YourConnectionString' with your actual connection string.
        optionsBuilder.UseNpgsql(@"Server=(localdb)\mssqllocaldb;Database=BankDB;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<SavingsAccount>().ToTable("SavingsAccounts");
        modelBuilder.Entity<SavingsAccount>().HasKey(sa => sa.AccountId);
        modelBuilder.Entity<SavingsAccount>().Property(sa => sa.CPR).IsRequired();
        modelBuilder.Entity<SavingsAccount>().Property(sa => sa.Name).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<SavingsAccount>().Property(sa => sa.Balance).HasColumnType("decimal(18,2)");
    }
    
}