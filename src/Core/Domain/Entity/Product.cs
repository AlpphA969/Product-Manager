using Domain.Base;

namespace Domain.Entity;

public class Product : BaseEntity
{
    public Product() :base(){ }

    public Product(string name , string color , List<ProductCategory>productCategories  , int instockcount) : base()

    {
        Name = name;
        Color = color;
        InStockCount = instockcount;
        ProductCategories = productCategories;

    }


    public string Name { get; set; }
    public string? Description { get; set; }
    public decimal? Price { get; set; }
    public string? ImageUrl { get; set; }
    public string Color { get; set; }
    public int InStockCount { get; set; }
    public List<ProductCategory>? ProductCategories { get; set; } = new List<ProductCategory>();
}