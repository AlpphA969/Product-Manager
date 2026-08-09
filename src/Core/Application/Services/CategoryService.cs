using Application.Abstraction;
using AutoMapper;
using Domain.Entity;
using FluentResults;
using Microsoft.Identity.Client;
using Models.ViewModel;
using Persistence;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application.Services
{
    public class CategoryService : ICategoryService
    {
        public CategoryService(IMapper mapper, IUnitOfWork unitofwork)
        {
            Mapper = mapper;
            UnitOfWork = unitofwork;

        }
        private IMapper Mapper { get; }
        private IUnitOfWork UnitOfWork { get; }
        public async Task<Result> AddCategoryAsync(CategoryViewModel model)
        {
            var result = new Result();
            if (model.Name == null|| model.Name.Length==0)
            {
               return   result.WithError("name cant be null");
            }
            var CategoryCheck = await UnitOfWork.CategoryRepository.FindByNameAsync(model.Name);
            if (CategoryCheck != null)
            {
                return result.WithError(model.Name + "    Category " + "Already Exists");
            }
            var category = Mapper.Map<Category>(model);
            await UnitOfWork.CategoryRepository.AddAsync(category );
             await UnitOfWork.SaveChangesAsync();
            return result;
            
        }
        public async Task<Result> UpdateByNameAsync(CategoryViewModel model  , Guid id )
        {
            var result = new Result();
            var category = await UnitOfWork.CategoryRepository.FindByIdAsync(id);
            if (category == null)
            {
                return result.WithError("Category Not Found");
            }
            if (model.Name == string.Empty)
            {
                return result.WithError("name requierd!");
            }
            
            category.Name = model.Name;
            category.UpdateDateTime = DateTime.Now;
            await UnitOfWork.CategoryRepository.UpdateByNameAsync(category);
            await UnitOfWork.SaveChangesAsync();
            return result;
            

        }
        public async Task<Result> RemoveByIdAsync(Guid id)
        {
            var result = new Result();
            var category = await UnitOfWork.CategoryRepository.FindByIdAsync(id);
            if (category == null)
            {
                return result.WithError("Category Not Found");
            }
            await UnitOfWork.CategoryRepository.RemoveByIdAsync(id);
            await UnitOfWork.SaveChangesAsync();
            return result.WithSuccess("Category Deleted");
        }
        public async Task<Result> ActiveAsync(Guid id)
        {
            var result = new Result();
            var category = await UnitOfWork.CategoryRepository.FindByIdAsync(id);
            if (category == null)
            {
                return result.WithError("Category Not Found");
            }
            if (category.IsActive)
            {
                return result.WithError("This Category Is Already Active!");
            }
            category.IsActive = true;
            await UnitOfWork.CategoryRepository.UpdateAsync(category);
            await UnitOfWork.SaveChangesAsync();
            return result;
        }
        public async Task<Result> InActiveAsync(Guid id)
        {
            var result = new Result();
            var category = await UnitOfWork.CategoryRepository.FindByIdAsync(id);
            if (category == null)
            {
                return result.WithError("Category Not Found");
            }
            if (category.IsActive==false)
            {
                return result.WithError("This Category Is Already InActive!");
            }
            category.IsActive = false;
            await UnitOfWork.CategoryRepository.UpdateAsync(category);
            await UnitOfWork.SaveChangesAsync();
            return result;
        }
    }
}
