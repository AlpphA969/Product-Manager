using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Models.ViewModel
{
    
    public class ProductViewModel : Base.BaseViewModel
    {
        public ProductViewModel(string color, string name) : base()
        {
            Color = color;
            Name = name;

        }
        public string Name { get; set; }
        public string? Description { get; set; }
        public decimal? Price { get; set; }
        public string? ImageUrl { get; set; }
        public string Color { get; set; }
        public int InStockCount { get; set; }
        public List<string>? CategoriesId { get; set; } = new List<string>();
       



    }
}
