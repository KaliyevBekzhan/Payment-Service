namespace Domain.Entity;

public class Currency
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public decimal ConversionRate { get; set; }
    
    // Many to One
    public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
}