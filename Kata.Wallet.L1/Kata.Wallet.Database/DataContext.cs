using Kata.Wallet.Domain;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace Kata.Wallet.Database;

public class DataContext : DbContext
{
    public DataContext(DbContextOptions<DataContext> options) : base(options)
    {
    }

    public DbSet<Domain.Wallet> Wallets { get; set; }
    public DbSet<Transaction> Transactions { get; set; }

    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        // Configuración de Transaction
        modelBuilder.Entity<Transaction>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Amount).HasPrecision(16, 2).IsRequired();
            entity.HasOne(x => x.WalletIncoming)
                .WithMany(x => x.IncomingTransactions)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasOne(x => x.WalletOutgoing)
                .WithMany(x => x.OutgoingTransactions)
                .OnDelete(DeleteBehavior.NoAction);
        });

        // Configuración de Wallet
        modelBuilder.Entity<Domain.Wallet>(entity =>
        {
            entity.HasKey(x => x.Id);
            entity.Property(x => x.Balance).HasPrecision(16, 2).IsRequired();
            entity.Property(x => x.Currency).IsRequired();
            entity.HasMany(x => x.IncomingTransactions)
                .WithOne(x => x.WalletIncoming)
                .OnDelete(DeleteBehavior.NoAction);
            entity.HasMany(x => x.OutgoingTransactions)
                .WithOne(x => x.WalletOutgoing)
                .OnDelete(DeleteBehavior.NoAction);
        });
    }
}
