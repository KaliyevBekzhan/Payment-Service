namespace Domain.Entity;

public class User
{
    public int Id { get; set; }
    public string WalletNumber { get; set; } = null!;
    public decimal Account { get; set; }
    public string Password { get; set; } = null!;
    public string IIN { get; set; } = null!;
    public string Name { get; set; } = null!;
    public int RoleId { get; set; }
    
    // One to Many
    public virtual Role Role { get; set; } = null!;
    
    // Many to One
    public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    public virtual ICollection<Payment> ChangedPayments { get; set; } = new HashSet<Payment>();
    public virtual ICollection<TopUp> TopUps { get; set; } = new HashSet<TopUp>();
}