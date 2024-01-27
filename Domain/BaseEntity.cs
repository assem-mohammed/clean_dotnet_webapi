namespace Domain;

public class BaseEntity<T>
{
    public T Id { get; set; } = default!;
    public DateTime DateCreated { get; set; } = DateTime.UtcNow;
    public DateTime? DateUpdated { get; set; }
    public DateTime? DateRemoved { get; set; }
    public string ModifiedBy { get; set; } = default!;
    public bool? IsActive { get; set; }
    public bool? IsDeleted { get; set; }
}
