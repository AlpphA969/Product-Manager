using Application.Abstraction;
using AutoMapper;
using Domain.Entity;
using FluentResults;
using Models.ViewModel;
using Persistence;
using Persistence.Abstraction;

namespace Application.Services;

public class ProductService : IProductService
{
    public ProductService(IMapper mapper, IUnitOfWork unitOfWork)
    {
        Mapper = mapper;
        UnitOfWork = unitOfWork;
        ProductRepository = unitOfWork.ProductRepository;
        CategoryRepository = unitOfWork.CategoryRepository;
        ProductCategoryRepository = unitOfWork.ProductCategoryRepository;

    }
    private IMapper Mapper { get; }

    IProductRepository ProductRepository { get; }
    ICategoryRepository CategoryRepository { get; }
    IProductCategoryRepository ProductCategoryRepository { get; }
    private IUnitOfWork UnitOfWork { get; }
    public async Task<Result> AddProductAsync(ProductViewModel productviewmodel)
    {

        var result = new Result();

        var product = Mapper.Map<Product>(productviewmodel);

        // اضاف کردن کتگوری ای دی ها به پروداکت کتگوری
        // چک کردن اینکه ایا این کتگوری ها وجود دارن یا نه 

      
        var categories = await CategoryRepository.FindByCategoriesId(productviewmodel.CategoriesId);

        if (categories.Count != productviewmodel.CategoriesId.Count)
        {
            return result.WithError("Enter Proper Category Id");
        }

        foreach (var categoryid in productviewmodel.CategoriesId)
        {
            product.ProductCategories.Add(new ProductCategory(productid: product.Id, categoryid: Guid.Parse(categoryid)));

        }
        await ProductRepository.AddAsync(product);
        await UnitOfWork.SaveChangesAsync();
        return result.WithSuccess("Product Created");


    }

    public async Task<Result> DeleteAsync(Guid id)
    {
        var result = new Result();
        var product = await ProductRepository.FindByIdAsync(id);

        if (product == null)
        {
             return result.WithError("Product Not found");
        }
        // حذف از جدول پروداکت
        await ProductRepository.RemoveAsync(product);
        //حذف از جدول پروداکت کتگوری
        var productcatgorylist = await ProductCategoryRepository.FindAllByProductIdAsync(id);
        await UnitOfWork.ProductCategoryRepository.DeleteRangeAsync(productcatgorylist);
        await UnitOfWork.SaveChangesAsync();
        return result;



    }



    public async Task<Result<ProductViewModel>> FindByIdAsync(Guid id)
    {
        var result = new Result<ProductViewModel>();



        var product = await ProductRepository.FindByIdAsync(id);



        if (product == null)
        {
            return result.WithError("Product not found");
        }
        var categoriesid = await ProductCategoryRepository.FindCategoriesByProductIdAsync(id);
        if (categoriesid.Count == 0)
        {
            return result.WithError("This Product Doesn't Have a Category");
        }
        var anyactive = await CategoryRepository.AnyActiveAsync(categoriesid);
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

        var product = await ProductRepository.FindByIdAsync(id);
        if (product == null)
        {
            return result.WithError("product not found");
        }
        if (model.Name.Length == 0 || model.Color.Length == 0)
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
        await ProductRepository.UpdateAsync(product);
        await UnitOfWork.SaveChangesAsync();
        return result;


    }

    public async Task<Result<PageResultViewModel<ProductViewModel>>> GetAllProductsAsync(ProductFiltersViewModel query , int page , int pagesize, CancellationToken cancelationToke)
    {
        var result = new Result<PageResultViewModel<ProductViewModel>>();
        if(query.MinPrice >= query.MaxPrice)
        {
            return result;
        }
        var pageresult = await ProductRepository.GetAllAsync(query, page, pagesize, cancelationToke);
        var pageresultviewmodel = Mapper.Map<PageResultViewModel<ProductViewModel>>(pageresult);
        return pageresultviewmodel;

   
    }



    public async Task<Result> UpdatePriceAsync(UpdatePriceViewModel updatePriceViewModel, Guid id)
    {
        var result = new Result();
        if (updatePriceViewModel.Price == null)
        {
            return result.WithError("price requerd");
        }
        var product = await ProductRepository.FindByIdAsync(id);
        if (product == null)
        {
             return result.WithError("Not Found");

        }
        product.Price = updatePriceViewModel.Price;
        await ProductRepository.UpdateAsync(product);
        await UnitOfWork.SaveChangesAsync();
        return result;

    }

    public async Task<Result> UpdateInStockCountAsync(UpdateInStockCountViewModel updateInStockCountViewModel, Guid id)
    {
        var result = new Result();
        if (updateInStockCountViewModel.InStockCount == 0)
        {
             return result.WithError("InStockCount Requerd");

        }
        var product = await ProductRepository.FindByIdAsync(id);
        if (product == null)
        {
           return result.WithError("Not Found");
        }
        product.InStockCount = updateInStockCountViewModel.InStockCount;
        await ProductRepository.UpdateInStockCountAsync(product);
        await UnitOfWork.SaveChangesAsync();
        return result;
    }
    public async Task<Result> AddCategoryToTheProduct(Guid id, List<string> categoriesid)
    {
        var result = new Result();
       
        if (id == Guid.Empty)
        {
            return result.WithError("Id requierd!!");
        }
        if (categoriesid.Any(x => x == string.Empty))
        {
            return result.WithError("categoriesid list can't be empty");
        }
        var product = await ProductRepository.FindByIdAsync(id);

        if (product == null)
        {
            return result.WithError("Product Not Found");
        }
        //خالی نبودن لیست


        // چک کردن موجودیت کنگوری
        var categories = await CategoryRepository.FindByCategoriesId(categoriesid);
        if (categories.Count != categoriesid.Count)
        {
            return result.WithError("Enter Proper Category Id");
        }
        var ProductCategoryList = new List<ProductCategory>();
        // چک کردن تکراری نبودن کتگوری 
        var DatabaseCategory = await ProductCategoryRepository.FindCategoriesByProductIdAsync(id);
        var newCategoryIds = categoriesid.Except(DatabaseCategory);
        foreach (var x in newCategoryIds)
        {
            ProductCategoryList.Add(new ProductCategory(productid: id, categoryid: Guid.Parse(x)));
        }


        await ProductCategoryRepository.AddRangeAsync(ProductCategoryList);





        await UnitOfWork.SaveChangesAsync();
        return result;





    }
    public async Task<Result> DeleteProductCategoriesAsync(Guid id, List<string> categoriesid)
    {
        var result = new Result();
        var product = await ProductRepository.FindByIdAsync(id);
        if (product == null)
        {
            return result.WithError("Product Not Found");
        }
        if (categoriesid.Any(x => x == string.Empty))
        {
            return result.WithError("Categories Can't Be Empty");
        }
        
        // چک کردن اینکه ایا این کتگوری هایی که فرستاده وجود دارن یا نه 
        var category =  await CategoryRepository.FindByCategoriesId(categoriesid);
        var wrongcategoriesid = categoriesid.Except(category);
        if(categoriesid.Count != category.Count)
        {
            return result.WithError(wrongcategoriesid.ToString() + "these categories dosen't exist" );
        }

        // چک کردن اینکه ایا این پروداکت این کتگوری هارو داره که میخاد پاک بشه 
        var categories = await ProductCategoryRepository.FindCategoriesByProductIdAsync(id);
        // چک کردن اینکه ایا این پروداکت دارای کتگورهای ورودی هست یا نه 
        bool checklist = true;
        foreach (var x in categoriesid)
        {
            if (!(categories.Contains(x)))
            {
                checklist = false;
                break;
            }
        }

        if (checklist==false)
        {
            return result.WithError("Inset Proper Categories");
        }
        //گرفتن کتگوری های پروداکت و فیلتر کردن اونایی که میخایم پاک کنیم 
        var productcategorylist = await ProductCategoryRepository.FindAllByProductIdAsync(id);
        var filterdproductcategorylist = productcategorylist
            .Where(x => categoriesid.Contains(x.CategoryId.ToString())).ToList(); 
        

        await ProductCategoryRepository.DeleteRangeAsync(filterdproductcategorylist);
        await UnitOfWork.SaveChangesAsync();
        return result;





    }

    public async Task<Result<PageResultViewModel<ProductViewModel>>> PaginationGet(int page, int pagesize)
    {
        var result = new Result<PageResultViewModel<ProductViewModel>>();
        if (pagesize < 1 || pagesize > 50)
        {

            return result.WithError("pagesize must be between 1 to 50 ");

        }
        if (page <= 0)
        {
            return result.WithError("page must be more than 0");

        }
        int totalcount = await ProductRepository.CountAsync();
        int pagecount = (int)Math.Ceiling((double)totalcount / pagesize);
        var products = await ProductRepository.PaginationGet(page: page, pagesize: pagesize);
        if (products == null)
        {
            return result.WithError("Nothing Found!");
        }
        var models = Mapper.Map<List<ProductViewModel>>(products);
        var pageresults = new PageResultViewModel<ProductViewModel>()
        {
            data = models,
            TotalCount = totalcount,
            PageIndex = page,
            PageCount = pagecount,

        };
        return result.WithValue(pageresults);





    }
}






