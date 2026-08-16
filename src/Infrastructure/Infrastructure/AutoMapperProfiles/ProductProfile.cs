
using Domain.Entity;
using Models.ViewModel;

namespace Infrastructure.AutoMapperProfiles
{
    public class ProductProfile :AutoMapper.Profile
    {
        public ProductProfile():base()
        {
            CreateMap<ProductViewModel, Product>().ReverseMap();

            CreateMap<PageDataModel<Product>, PageResultViewModel<ProductViewModel>>();
                
            

        }

    }
}
