namespace Domain.Entity;

public class TransactionsView
{
    public int TransId { get; set; }
    public int UserId { get; set; }
    public decimal OriginalAmount { get; set; }
    public string CurrencyName { get; set; }
    public decimal AmountInTenge { get; set; }
    public string Comment { get; set; }
    public string StatusName { get; set; }
    public DateTime CreatedAt { get; set; }
    public string TransType { get; set; }
}