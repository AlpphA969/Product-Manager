using Domain.Entity;
using Models.ViewModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure.AutoMapperProfiles
{
    public class CategoryProfile:AutoMapper.Profile
    {
        public CategoryProfile()
        {
            CreateMap<CategoryViewModel, Category>();
        }

    }
}
