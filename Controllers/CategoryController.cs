using Microsoft.AspNetCore.Mvc;
using Blog.Data;
using Microsoft.EntityFrameworkCore;
using Blog.Models;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context)
        {
            var categories = await context.Categories.ToListAsync();
            return Ok(categories);
        }

        
        [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context)
        {
            var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
            if (category == null)
                return NotFound();
            return Ok(category);
            
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
           [FromBody] Category model,
            [FromServices] BlogDataContext context)
        {
           await context.Categories.AddAsync(model);
           await context.SaveChangesAsync(); 
           return Created($"v1/categories/{model.Id}", model);
        }

         [HttpPut("v1/categories{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] Category model,
            [FromServices] BlogDataContext context)
        {
           var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
           if (category == null)
                return NotFound();
              category.Name = model.Name;
              category.Slug = model.Slug;

              context.Categories.Update(category);  // Atualiza o registro
                await context.SaveChangesAsync(); 
                return NoContent();
        }

         [HttpDelete("v1/categories{id:int}")]

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
