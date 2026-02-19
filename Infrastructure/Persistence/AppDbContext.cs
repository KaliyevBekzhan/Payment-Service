using Domain.Entity;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure;

public class AppDbContext : DbContext
{
    public AppDbContext(DbContextOptions<AppDbContext> options)
        : base(options)
    {
        
    }
    
    public DbSet<Payment> Payments { get; set; }
    public DbSet<User> Users { get; set; }
    public DbSet<Role> Roles { get; set; }
    public DbSet<Status> Statuses { get; set; }
    public DbSet<Currency> Currencies { get; set; }
    public DbSet<TopUp> TopUps { get; set; }
    
    public DbSet<TransactionsView> Transactions { get; set; }
    
    protected override void OnModelCreating(ModelBuilder modelBuilder)
    {
        base.OnModelCreating(modelBuilder);

        modelBuilder.Entity<Payment>(entity =>
        {
            entity.HasOne(p => p.User)
                .WithMany(u => u.Payments)
                .HasForeignKey(p => p.UserId);
            
            entity.HasOne(p => p.Status)
                .WithMany(s => s.Payments)
                .HasForeignKey(p => p.StatusId);
            
            entity.HasOne(p => p.Currency)
                .WithMany(c => c.Payments)
                .HasForeignKey(p => p.CurrencyId);

            entity.HasOne(p => p.Changer)
                .WithMany(u => u.ChangedPayments)
                .HasForeignKey(p => p.ChangerId);
            
            entity.ToTable("payment", t =>
            {
                t.HasCheckConstraint(
                    "ck_original_amount_not_negative",
                    "original_amount > 0");

                t.HasCheckConstraint(
                    "ck_amount_in_tenge_not_negative",
                    "amount_in_tenge > 0"
                );
            });
            
            entity.Property(p => p.OriginalAmount).HasPrecision(18, 2).IsRequired();
            entity.Property(p => p.AmountInTenge).HasPrecision(18, 2).IsRequired();
        });

        modelBuilder.Entity<User>(entity =>
        {
            entity.HasOne(u => u.Role)
                .WithMany(r => r.Users)
                .HasForeignKey(u => u.RoleId);
            
            entity.ToTable("users", t => 
            {
                t.HasCheckConstraint("ck_user_name_not_empty", "name IS NOT NULL AND name <> ''");
                t.HasCheckConstraint("ck_user_iin_format", "iin ~ '^[0-9]{12}$'");
            });
            
            entity.HasIndex(u => u.IIN)
                .IsUnique()
                .HasDatabaseName("uq_user_iin");
            
            entity.HasIndex(u => u.WalletNumber)
                .IsUnique()
                .HasDatabaseName("uq_user_wallet_number");
        });

        modelBuilder.Entity<Role>(entity =>
        {
            entity.ToTable("role");
            
            entity.HasMany(r => r.Users)
                .WithOne(u => u.Role)
                .HasForeignKey(u => u.RoleId);
            
            entity.HasIndex(r => r.Name)
                .IsUnique()
                .HasDatabaseName("uq_role_name");
        });
        
        modelBuilder.Entity<Status>(entity =>
        {
            entity.ToTable("status");
            
            entity.HasMany(s => s.Payments)
                .WithOne(p => p.Status)
                .HasForeignKey(p => p.StatusId);
            
            entity.HasIndex(s => s.Name)
                .IsUnique()
                .HasDatabaseName("uq_status_name");
        });

        modelBuilder.Entity<Currency>(entity =>
        {
            entity.HasMany(c => c.Payments)
                .WithOne(p => p.Currency)
                .HasForeignKey(p => p.CurrencyId);

            entity.HasIndex(c => c.Name)
                .IsUnique()
                .HasDatabaseName("uq_currency_name");

            entity.ToTable("currency", t => t.HasCheckConstraint(
                "ck_conv_rate_valid",
                "conv_rate > 0"));
        });

        modelBuilder.Entity<TopUp>(entity =>
        {
            entity.ToTable("topup", t =>
            {
                t.HasCheckConstraint("ck_topup_og_amount_not_negative", "original_amount > 0");
                t.HasCheckConstraint("ck_topup_amount_in_tenge_not_negative", "amount_in_tenge > 0");
            });
            entity.HasOne(t => t.User)
                .WithMany(u => u.TopUps)
                .HasForeignKey(t => t.UserId);
            
            entity.HasOne(t => t.Currency)
                .WithMany(c => c.TopUps)
                .HasForeignKey(t => t.CurrencyId);
            
            entity.HasOne(t => t.Status)
                .WithMany(s => s.TopUps)
                .HasForeignKey(t => t.StatusId);
        });
        
        modelBuilder.Entity<TransactionsView>(entity =>
        {
            entity.HasNoKey();
            entity.ToView("v_alltransactions");
        });
    }
}
