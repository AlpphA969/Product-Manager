using Domain.Base;

namespace Domain.Entity;

public class Category : BaseEntity
{
    public string Name { get; set; }

    public List<ProductCategory> ProductCategories { get; set; }
    
    
}
