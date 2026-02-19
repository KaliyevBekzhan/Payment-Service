namespace Domain.Entity;

public class Status
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool Viewable { get; set; }
    
    // Many to One
    public virtual ICollection<Payment> Payments { get; set; } = new HashSet<Payment>();
    public virtual ICollection<TopUp> TopUps { get; set; } = new HashSet<TopUp>();
}