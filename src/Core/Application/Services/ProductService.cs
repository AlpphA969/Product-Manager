using Application.Abstraction;
using FluentResults;
using Persistence;
using AutoMapper;
using Models.ViewModel;
using Domain.Entity;
using System.Text.Unicode;
using Persistence.Repository;

namespace Application.Services;

public class ProductService : IProductService
{
    public ProductService(IMapper mapper , IUnitOfWork unitofwork)
    {
        Mapper = mapper;
        UnitOfWork = unitofwork;
        
    }
    private IMapper Mapper { get; }
    private IUnitOfWork UnitOfWork { get; }
    public async Task<Result> AddProductAsync(ProductViewModel productviewmodel)
    {
        
        var result = new Result();
        
        var product = Mapper.Map<Product>(productviewmodel);

        // اضاف کردن پرداکت ای دی ها به پروداکت کتگوری
        var categories = await UnitOfWork.CategoryRepository.FindByCategoriesId(productviewmodel.CategoriesId);
        
        if (categories.Count != productviewmodel.CategoriesId.Count)
        {
            return result.WithError("Enter Proper Category Id");
        }
        
        foreach (var categoryid in productviewmodel.CategoriesId)
        {
            product.ProductCategories.Add(new ProductCategory(productid: product.Id, categoryid:Guid.Parse(categoryid)));

        }
        await UnitOfWork.ProductRepository.AddAsync(product);
        await UnitOfWork.SaveChangesAsync();    
        return result.WithSuccess("Product Created");

        
    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var result = new Result();
        var product = await UnitOfWork.ProductRepository.FindByIdAsync(id);
        
        if (product == null)
        {
            result.WithError("Product Not found");
        }
        // حذف از جدول پروداکت
        await UnitOfWork.ProductRepository.RemoveAsync(product);
        //حذف از جدول پروداکت کتگوری
        var productcatgorylist = await UnitOfWork.ProductCategoryRepository.FindAllByProductIdAsync(id);
        await UnitOfWork.ProductCategoryRepository.DeleteRangeAsync(productcatgorylist);
        await UnitOfWork.SaveChangesAsync();
        return result;


        
    }

  

    public async Task<Result<ProductViewModel>> FindByIdAsync(Guid id)
    {
        var result = new Result<ProductViewModel>();
        
        

        var product =  await UnitOfWork.ProductRepository.FindByIdAsync(id);
       
        

        if (product == null)
        {
             return result.WithError("Product not found");
        }
        var categoriesid = await UnitOfWork.ProductCategoryRepository.FindCategoriesByProductIdAsync(id);
        if (categoriesid.Count == 0)
        {
            return result.WithError("This Product Doesn't Have a Category");
        }
        var anyactive = await UnitOfWork.CategoryRepository.AnyActiveAsync(categoriesid);
        if (anyactive == false)
        {
            return result.WithError("Not Found(No Category Active)");
        }
        

        
        var productmodel = Mapper.Map<ProductViewModel>(product);
        

        productmodel.CategoriesId.AddRange(categoriesid);
        return result.WithValue(productmodel);

    }

    public async Task<Result> UpdateAsync(ProductViewModel model, Guid id)
    {
        var result = new Result();
        
        var product = await UnitOfWork.ProductRepository.FindByIdAsync(id);
        if (product == null)
        {
             return result.WithError("product not found");
        }
        if (model.Name.Length == 0 || model.Color.Length==0)
        {
            return result.WithError("name and color requierd");

        }
        
        
       
        product.Name = model.Name;
        product.Color = model.Color;
        
        if (model.Description != null)
        {
            product.Description = model.Description;
        }
        if (model.ImageUrl != null)
        {
            product.ImageUrl = model.ImageUrl; 
        }
        product.UpdateDateTime = DateTime.Now;
        await UnitOfWork.ProductRepository.UpdateAsync(product);
        await UnitOfWork.SaveChangesAsync();
        return result;


    }

    public async Task<Result<List<ProductViewModel>>> GetAllProductsAsync(ProductFiltersViewModel query , CancellationToken cancelationToke)
    {

        var result = new Result<List<ProductViewModel>>();

        if (query.MaxPrice != null && query.MinPrice != null && query.MaxPrice < query.MinPrice)
        {

            result.WithError("Minprice cant be more than Maxprice");
            return result;
        }


        var products = await UnitOfWork.ProductRepository.GetAllAsync(query , cancelationToke);


        var productViewModel = Mapper.Map<List<ProductViewModel>>(products);

        result.WithValue(productViewModel);

        return result;

    }

   

    public async Task<Result> UpdatePriceAsync(UpdatePriceViewModel updatePriceViewModel ,  Guid id )
    {
        var result = new Result();
        if (updatePriceViewModel.Price == null)
        {
            return result.WithError("price requerd");
        }
        var product = await UnitOfWork.ProductRepository.FindByIdAsync(id);
        if (product == null)
        {
            result.WithError("Not Found");

        }
        product.Price = updatePriceViewModel.Price;
        await UnitOfWork.ProductRepository.UpdateAsync(product);
        await UnitOfWork.SaveChangesAsync();
        return result;
        
    }

    public async Task<Result> UpdateInStockCountAsync(UpdateInStockCountViewModel updateInStockCountViewModel, Guid id)
    {
        var result = new Result();
        if (updateInStockCountViewModel.InStockCount==0)
        {
            result.WithError("InStockCount Requerd");

        }
        var product = await UnitOfWork.ProductRepository.FindByIdAsync(id);
        if (product == null)
        {
            result.WithError("Not Found");
        }
        product.InStockCount = updateInStockCountViewModel.InStockCount;
        await UnitOfWork.ProductRepository.UpdateInStockCountAsync(product);
        await UnitOfWork.SaveChangesAsync();
        return result; 
    }
    public async Task<Result> AddCategoryToTheProduct(Guid id, List<string> categoriesid)
    {
        var result = new Result();
        Console.WriteLine("function dare ejra mishe!!!!!!!!!!!!!!!1");
        if (id == Guid.Empty)
        {
            return result.WithError("Id requierd!!");
        }
        if (categoriesid.Any(x => x == string.Empty))
        {
            return result.WithError("categoriesid list can't be empty");
        }
        var product = await UnitOfWork.ProductRepository.FindByIdAsync(id);
        
        if (product == null)
        {
            return result.WithError("Product Not Found");
        }
        //خالی نبودن لیست
        
            
        // چک کردن موجودیت کنگوری
        var categories = await UnitOfWork.CategoryRepository.FindByCategoriesId(categoriesid);
        if(categories.Count != categoriesid.Count)
        {
            return result.WithError("Enter Proper Category Id");
        }
        var ProductCategoryList = new List<ProductCategory>();
        // چک کردن تکراری نبودن کتگوری 
        var DatabaseCategory = await UnitOfWork.ProductCategoryRepository.FindCategoriesByProductIdAsync(id);
        var newCategoryIds = categoriesid.Except(DatabaseCategory);
        foreach(var x in newCategoryIds)
        {
            ProductCategoryList.Add(new ProductCategory(productid:id, categoryid:Guid.Parse(x)));
        }


        await UnitOfWork.ProductCategoryRepository.AddRangeAsync(ProductCategoryList);



        
        
        await UnitOfWork.SaveChangesAsync();    
        return result;





    }
    public async Task<Result> DeleteProductCategoriesAsync(Guid id , List<string> categoriesid)
    {
        var result = new Result();
        var product = await UnitOfWork.ProductRepository.FindByIdAsync(id);
        if (product == null)
        {
            return result.WithError("Product Not Found");
        }
        if (categoriesid.Any(x => x == string.Empty))
        {
            return result.WithError("Categories Can't Be Empty");
        }
        var categories = await UnitOfWork.ProductCategoryRepository.FindCategoriesByProductIdAsync(id);
        if (categories.Count != categoriesid.Count)
        {
            return result.WithError("Inset Proper Categories");
        }


        var productcategorylist = await UnitOfWork.ProductCategoryRepository.FindAllByProductIdAsync(id);
        if (productcategorylist.Count != categoriesid.Count)
        {
            return result.WithError("Enter Proper Categories");
        }
       
        await UnitOfWork.ProductCategoryRepository.DeleteRangeAsync(productcategorylist);
        await UnitOfWork.SaveChangesAsync();
        return result;

        

        
        
    }


}