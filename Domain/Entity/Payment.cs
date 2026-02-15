namespace Domain.Entity;

public class Payment
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public decimal Amount { get; set; }
    public string WalletNumber { get; set; } = null!;
    public int Account { get; set; }
    public int CurrencyId { get; set; }
    public string Comment { get; set; } = null!;
    public int StatusId { get; set; }
    public DateTime CreatedAt { get; set; }
    
    // One to Many
    public virtual User User { get; set; } = null!;
    public virtual Status Status { get; set; } = null!;
    public virtual Currency Currency { get; set; } = null!;
}