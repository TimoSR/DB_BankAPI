using API.Features.CreateAccount.Domain;
using Microsoft.EntityFrameworkCore;

namespace API.Features.CreateAccount.Contexts;

public class AccountContext : DbContext
{
    public DbSet<Account> SavingsAccounts { get; set; }

    protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
    {
        // Replace 'YourConnectionString' with your actual connection string.
        optionsBuilder.UseNpgsql(@"Server=(localdb)\mssqllocaldb;Database=BankDB;Trusted_Connection=True;");
    }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Account>().ToTable("SavingsAccounts");
        modelBuilder.Entity<Account>().HasKey(sa => sa.Id);
        modelBuilder.Entity<Account>().Property(sa => sa.CPR).IsRequired();
        modelBuilder.Entity<Account>().Property(sa => sa.Name).IsRequired().HasMaxLength(100);
        modelBuilder.Entity<Account>().Property(sa => sa.Balance).HasColumnType("decimal(18,2)");
    }
    
}