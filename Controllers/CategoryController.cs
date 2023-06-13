using Microsoft.AspNetCore.Mvc;
using Blog.Data.Mappings;
using Blog.Models;
using Microsoft.EntityFrameworkCore;

namespace Blog.Controllers
{
    [ApiController]
    public class CategoryController : ControllerBase
    {
        [HttpGet("categories")]
        public IActionResult Get(
            [FromServices] BlogDataContext context)
        {
            va categories = context.Categories.ToList();
            return Ok();
        }
    }
}
