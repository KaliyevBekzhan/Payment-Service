namespace Domain.Entity;

public class Role
{
    public int Id { get; set; }
    public string Name { get; set; } = null!;
    public bool IsAdmin { get; set; }
    
    // Many to One
    public virtual ICollection<User> Users { get; set; } = new HashSet<User>();
}