using Microsoft.AspNetCore.Mvc;
using Blog.Data;
using Microsoft.EntityFrameworkCore;
using Blog.Models;
using Blog.ViewModels;
namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    { [HttpGet("v1/categories")]
        public async Task<IActionResult> GetAsync(
            [FromServices] BlogDataContext context)
        {
            try
            {
                var categories = await context.Categories.ToListAsync();
                return Ok(new ResultViewModel<List<Category>>(categories));
            }
              catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("05XE8 - Não foi possível buscar as categorias"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<List<Category>>("05X04 - Falha interna no servidor"));
            }
        }
        
       [HttpGet("v1/categories/{id:int}")]
        public async Task<IActionResult> GetByIdAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context)
        {
            try
            {
                var category = await context
                    .Categories
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (category == null)
                    return NotFound(new ResultViewModel<Category>("Conteúdo não encontrado"));

                return Ok(new ResultViewModel<Category>(category));
            }
              catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("05XE8 - Não foi possível buscar a categoria"));
            }
            catch
            {
                return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
            }
        }

        [HttpPost("v1/categories")]
        public async Task<IActionResult> PostAsync(
           [FromBody] EditorCategoryViewModel model,
            [FromServices] BlogDataContext context)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState.Values.SelectMany(e => e.Errors));
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
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("05XE8 - Não foi possível criar a categoria"));
            }
           catch (Exception)
           {
               return StatusCode(500, new ResultViewModel<Category>("Falha interna no servidor"));
           }
        }

         [HttpPut("v1/categories/{id:int}")]
        public async Task<IActionResult> PutAsync(
            [FromRoute] int id,
            [FromBody] EditorCategoryViewModel model,
            [FromServices] BlogDataContext context)
        {
           try 
           {
               var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
               if (category == null)
                  return NotFound();
                  category.Name = model.Name;
                  category.Slug = model.Slug.ToLower();
                  context.Entry<Category>(category).State = EntityState.Modified;
                  await context.SaveChangesAsync();
                  return Created($"/v1/categories/{category.Id}", new ResultViewModel<Category>(category));
           }
           catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("05XE8 - Não foi possível alterar a categoria"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("05X11 - Falha interna no servidor"));
            }
        }

         [HttpDelete("v1/categories/{id:int}")]

        public async Task<IActionResult> DeleteAsync(
            [FromRoute] int id,
            [FromServices] BlogDataContext context)
        {
              try {
                var category = await context.Categories.FirstOrDefaultAsync(x => x.Id == id);
                if (category == null)
                    return NotFound();
                context.Categories.Remove(category);
                await context.SaveChangesAsync();
                return Ok(new ResultViewModel<Category>(category));
              }
                catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("05XE8 - Não foi possível deletar a categoria"));
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResultViewModel<Category>("05X11 - Falha interna no servidor"));
            }

        }
    }
}
