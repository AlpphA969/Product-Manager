namespace Domain.Base;

public abstract class BaseEntity : object
{
    

    public BaseEntity() : base()
    {
        Id = Guid.NewGuid();
        CreateDateTime = DateTime.Now;
        
        IsActive = true;
    }

    public Guid Id { get; set; }

    public DateTime CreateDateTime { get; set; }

    public DateTime UpdateDateTime { get; set; }

    public bool IsActive { get; set; }
}