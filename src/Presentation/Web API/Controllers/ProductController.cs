using Application.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModel;
namespace Web_API.Controllers
{
    [ApiController]
    [Route(template: "api/Products")]
    public class ProductController : ControllerBase
    {
        public ProductController(IProductService productService)
        {
            ProductService = productService;

        }

        private IProductService ProductService { get; }
        [HttpPost(template: "addproduct")]

        public async Task<IActionResult> AddProductAsync([FromBody] ProductViewModel model)
        {



            if (model == null)
            {
                return BadRequest("product cant be null");
            }
            var result = await ProductService.AddProductAsync(model);
            return Ok(result);
        }
        [HttpGet(template: "getproduct/{id:guid}")]
        public async Task<IActionResult> FindByIdAsync(Guid id)
        {

            var result = await ProductService.FindByIdAsync(id);
            if (result.IsFailed)
            {
                return NotFound(result.Errors);
            }
            return Ok(result.Value);

        }
        [HttpPut(template: "updateproduct/{id:guid}")]
        public async Task<IActionResult> UpdateAsync(Guid id, [FromBody] ProductViewModel bodymodel)
        {


            var result = await ProductService.UpdateAsync(bodymodel, id);
            if (result.IsFailed)
            {
                return BadRequest(result);
            }
            return Ok("Product Updated!");

        }
        [HttpDelete(template: "deleteproduct/{id:guid}")]
        public async Task<IActionResult> DeleteAsync(Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            var result = await ProductService.DeleteAsync(id);
            if (result.IsFailed)
            {
                return BadRequest();

            }
            return Ok("product deleted");


        }



        [HttpPatch(template: "updateprice/{id:guid}")]
        public async Task<IActionResult> UpdatePriceAsync([FromBody] UpdatePriceViewModel updatepriceviewmodel, Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            if (updatepriceviewmodel == null)
            {
                return BadRequest();
            }
            var result = await ProductService.UpdatePriceAsync(updatepriceviewmodel, id);
            if (result.IsFailed)
            {
                return BadRequest(result);
            }
            return Ok("Price Updated!");

        }
        [HttpPatch(template: "updateinstockcount/{id:guid}")]
        public async Task<IActionResult> UpdateInStockCountAsync([FromBody] UpdateInStockCountViewModel updateInStockCountViewModel, Guid id)
        {
            if (id == Guid.Empty)
            {
                return BadRequest();
            }
            Console.WriteLine(updateInStockCountViewModel.InStockCount);

            var result = await ProductService.UpdateInStockCountAsync(updateInStockCountViewModel, id);
            if (result.IsFailed)
            {
                return BadRequest(result);
            }
            return Ok("Instockcount Updated!");

        }


        [HttpPost(template: "GetAllByFilter")]
        public async Task<IActionResult> GetAllByFilter([FromBody] ProductFiltersViewModel model ,  [FromQuery] int page, [FromQuery] int pagesize)
        {

            var result = await ProductService.GetAllProductsAsync(model , page , pagesize);

            if (result.IsSuccess)
            {

                return Ok(result.Value);
            }
            else
            {

                return BadRequest(error: result.Errors.FirstOrDefault());

            }


        }
        [HttpPatch(template: "AddCategoriesToTheProduct/{id:guid}")]
        public async Task<IActionResult> AddCategoryToTheProduct(Guid id, [FromBody] AddCategoryToTheProductViewModel model)
        {

            var result = await ProductService.AddCategoryToTheProduct(id, model.categoriesid);
            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }
            return Ok("categories added");
        }
        [HttpPatch(template: "deleteproductcategories/{id:guid}")]
        public async Task<IActionResult> DeletProductCategories(Guid id, [FromBody] AddCategoryToTheProductViewModel model)
        {
            var result = await ProductService.DeleteProductCategoriesAsync(id, model.categoriesid);
            if (result.IsFailed)
            {
                return BadRequest(result);
            }
            return Ok("categories deleted");


        }
        [HttpGet(template: "paginationget")]
        public async Task<IActionResult> PaginationGet([FromQuery]int page  ,[FromQuery]int pagesize)
        {
            
            var result = await ProductService.PaginationGet(page, pagesize);
            if (result.IsFailed)
            {
                return BadRequest(result.Errors);
            }
            return Ok(result.Value);

        }
    }
}
