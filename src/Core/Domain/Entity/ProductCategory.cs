using Domain.Base;

namespace Domain.Entity;

public class ProductCategory : BaseEntity
{
    public ProductCategory() { }
    public ProductCategory(Guid categoryid  , Guid productid ) : base()
    {
        CategoryId = categoryid ;
        ProductId = productid ;
        
    }
    
    public Guid ProductId { get; set; }
    public Guid CategoryId { get; set; }
    
    public virtual Product? Product { get; set; }
    public virtual Category? Category { get; set; }
}