namespace ThestralService.Infrastructure.EntityFrameworkCore.DbContext;

using ModelBuilder;
using Domain.Transfer.Entities;
using Microsoft.EntityFrameworkCore;

public class TransferDbContext : DbContext
{ 
    public DbSet<Transfer> Transfers { get; set; }
    
    public TransferDbContext(DbContextOptions<TransferDbContext> options) : base(options)
    {
    }

    protected override void OnModelCreating(Microsoft.EntityFrameworkCore.ModelBuilder modelBuilder)
    {
        modelBuilder.Entity<Transfer>().Configure();
    }
}
