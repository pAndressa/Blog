using Blog.Data;
using Blog.Extensions;
using Blog.Models;
using Blog.ViewModels;
using Blog.ViewModels.Categories;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Caching.Memory;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync(
            [FromServices]BlogDataContext context
        )
        {
            try
            {
                var categories = await context.Categories.ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("04EX01 - Falha interna no servidor"));
            }
        }

        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute]int id,
            [FromServices]BlogDataContext context
        )
        {
            try
            {
                var category = await context
                    .Categories
                    .FirstOrDefaultAsync(x=> x.Id == id);

                if(category == null)
                    return NotFound(new ResultViewModel<List<Category>>("Conteúdo não encontrado"));

                return Ok(new ResultViewModel<Category>(category));
            }
            catch
            {
                return StatusCode(500, "Falha interna no servidor");
            }
            
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
            [FromBody] EditorCategoryViewModel model,
            [FromServices]IMemoryCache cache,
            [FromServices]BlogDataContext context
        )
        {
            if(!ModelState.IsValid)
                return BadRequest(new ResultViewModel<Category>(ModelState.GetErrors()));

            try
            {
                var categories = cache.GetOrCreate("CategoriesCache", entry =>
                {
                    entry.AbsoluteExpirationRelativeToNow = TimeSpan.FromHours(1);
                    return GetCategories(context);
                })
                return Ok(new ResultViewModel<List<Category>(categories));
            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("05EX01 - Não foi possível incluir a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("05EXE10 - Falha interna no servidor"));
            }
        }

        private List<Category> GetCategories(BlogDataContext context)
        {
            return context.Categories.ToList();
        }

        [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute]int id,
            [FromBody] EditorCategoryViewModel model,
            [FromServices]BlogDataContext context
        )
        {
            try
            {
                var category = await context
                    .Categories
                    .FirstOrDefaultAsync(x=> x.Id == id);

                if(category == null)
                    return NotFound();

                category.Name = model.Name;
                category.Slug = model.Slug;

                context.Categories.Update(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("05EX02 - Não foi possível alterar a categoria"));
            }
            catch(Exception e)
            {
                return StatusCode(500, new ResultViewModel<Category>("05EXE11 - Falha interna no servidor"));
            }
        }

        [HttpDelete("v1/categories/{id:int}")]
        public async Task<IActionResult> DeleteAsync(
            [FromRoute]int id,
            [FromServices]BlogDataContext context
        )
        {
            try
            {
                var category = await context
                    .Categories
                    .FirstOrDefaultAsync(x=> x.Id == id);

                if(category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                context.Categories.Remove(category);
                await context.SaveChangesAsync();

                return Ok(new ResultViewModel<Category>(category));
            }
            catch(DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("05EX03 - Não foi possível excluir a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("05EXE12 - Falha interna no servidor"));
            }            
        }
    }
}