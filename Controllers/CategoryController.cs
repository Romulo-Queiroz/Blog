using Microsoft.AspNetCore.Mvc;
using Blog.Data;
using Microsoft.EntityFrameworkCore;
using Blog.Models;
using Blog.ViewModels;
namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context)
        {
            try {
                var categories = await context.Categories.AsNoTracking().ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível obter as categorias" });
            }
        }

        
        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context)
        {
            try 
            {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                return Ok(new ResultViewModel<Category>(category));
            }
            catch (Exception)
            {
                return BadRequest(new { message = "Não foi possível obter a categoria" });
            }
            
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
           [FromBody] EditorCategoryViewModel model,
            [FromServices] BlogDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest();
           try
           {
            var category = new Category{
                Id = 0,
                Name = model.Name,
                Slug = model.Slug.ToLower()
            };
                context.Categories.Add(category);
               await context.SaveChangesAsync();
               return Created($"/v1/categories/{category.Id}", category);
           }
           catch (Exception)
           {
               return BadRequest(new { message = "Não foi possível criar a categoria" });
           }
        }

         [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] EditorCategoryViewModel model,
            [FromServices] BlogDataContext context)
        {
           var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
           if (category == null)
                return NotFound();
              category.Name = model.Name;
              category.Slug = model.Slug;

              context.Categories.Update(category);  // Atualiza o registro
                await context.SaveChangesAsync(); 
                return Ok(category);
        }

         [HttpDelete("v1/categories/{id:int}")]

        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context)
        {
              var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
              if (category == null)
                 return NotFound();
                  context.Categories.Remove(category);  // Remove o registro
                 await context.SaveChangesAsync(); 
                 return NoContent();

        }
    }
}
