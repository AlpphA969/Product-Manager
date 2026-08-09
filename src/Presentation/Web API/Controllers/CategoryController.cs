using Application.Abstraction;
using Microsoft.AspNetCore.Mvc;
using Models.ViewModel;
using Persistence.Abstraction;

namespace Web_API.Controllers
{
    [ApiController]
    [Route("api/categories")]
    public class CategoryController : Controller
    {
       public CategoryController(ICategoryService categoryService )
        {
            CategoryService = categoryService;

        }
        private ICategoryService CategoryService { get; }
        [HttpPost(template:"addcategory")]
        public async Task<IActionResult> AddCategoryAsync([FromBody] CategoryViewModel model)
        {
           
            var result = await CategoryService.AddCategoryAsync(model);
            if (result.IsFailed)
            {
                return BadRequest(result);
            }
            return Ok("category created");
        }
        [HttpPatch(template:"updatecategory/{id:guid}")]
        public async Task<IActionResult> UpdateByNameAsync(CategoryViewModel model , Guid id)
        {
            var result = await CategoryService.UpdateByNameAsync(model, id );
            if (result.IsSuccess)
            {
                return Ok("category updated");
            }
            return BadRequest(result);
            
        }
        [HttpDelete(template:"deletecategory/{id:guid}")]
        public async Task<IActionResult> RemoveByIdAsync(Guid id)
        {
            var result = await CategoryService.RemoveByIdAsync(id);
            if (result.IsFailed)
            {
                return BadRequest(result);
            }
            return Ok("Category Deleted!");

        }
        [HttpPatch(template:"activecategory/{id:guid}")]
        public async Task<IActionResult> ActiveCategoryAsync(Guid id)
        {
            var result = await CategoryService.ActiveAsync(id);
            if (result.IsFailed)
            {
                return BadRequest(result);
            }
            return Ok("Category Activated!");

        }
        [HttpPatch(template: "inactivecategory/{id:guid}")]
        public async Task<IActionResult> InActiveCategoryAsync(Guid id)
        {
            var result = await CategoryService.InActiveAsync(id);
            if (result.IsFailed)
            {
                return BadRequest(result);
            }
            return Ok("Category InActivated!");

        }


    }
}
