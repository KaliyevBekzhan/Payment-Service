namespace Domain.Entity;

public class TopUp
{
    public int Id { get; set; }
    public int UserId { get; set; }
    public virtual User User { get; set; } = null!;
    
    public decimal Account { get; set; }
    public string WalletNumber { get; set; }
    public decimal OriginalAmount { get; set; }
    public decimal AmountInTenge { get; set; }
    
    public int CurrencyId { get; set; }
    public virtual Currency Currency { get; set; } = null!;
    public string CurrencyName { get; set; }
    
    public DateTime CreatedAt { get; set; } = DateTime.UtcNow;
    
    public int StatusId { get; set; }
    public virtual Status Status { get; set; } = null!;

    public string Comment { get; set; } = null!;
}