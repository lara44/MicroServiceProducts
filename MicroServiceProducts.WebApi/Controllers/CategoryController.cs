
using Application.Categories.Commands.CreateCategory;
using Application.Categories.Queries.GetCategoryAll;
using Application.Categories.Queries.GetCategoryById;
using MediatR;
using Microsoft.AspNetCore.Mvc;

namespace WebApi.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CategoryController : ControllerBase
    {
        private readonly IMediator _mediator;

        public CategoryController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("CreateCategory")]
        public async Task<IActionResult> CreateCategory([FromBody] CreateCategoryCommand command)
        {
            if (!ModelState.IsValid)
            {
                return BadRequest(ModelState);
            }

            var categoryId = await _mediator.Send(command);
            return Ok(new { id = categoryId });
        }
        
        [HttpGet("GetCategories")]
        public async Task<IActionResult> GetCategories()
        {
            var categories = await _mediator.Send(new GetCategoriesAllQuery());
            return Ok(categories);
        }

        [HttpGet("GetCategoryById/{id}")]
        public async Task<IActionResult> GetCategoryById(Guid id)
        {
            var category = await _mediator.Send(new GetCategoryByIdQuery(id));
            if (category == null)
            {
                throw new KeyNotFoundException($"La categoria con ID '{id}' no existe.");
            }
            return Ok(category);
        }
    }
}
