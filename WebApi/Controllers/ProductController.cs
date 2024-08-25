
using Application.Products.Commands;
using Application.Products.Queries;
using MediatR;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace WebApi.Controllers
{
    [ApiController]
    [Route("api/products")]
    public class ProductController : ControllerBase
    {
        private readonly IMediator _mediator;

        public ProductController(IMediator mediator)
        {
            _mediator = mediator ?? throw new ArgumentNullException(nameof(mediator));
        }

        [HttpPost("CreateProduct")]
        public async Task<IActionResult> CreateProduct([FromBody] CreateProductCommand command)
        {
            try
            {
                if (!ModelState.IsValid)
                {
                    return BadRequest(ModelState);
                }

                var productId = await _mediator.Send(command);
                return Ok(new { id = productId });
            }
            catch (DbUpdateException ex)
            {
                var errorMessage = "An error occurred while saving the product. Please ensure that the product name is unique.";
                errorMessage += $" Details: {ex.Message}";
                return BadRequest(new { message = errorMessage });
            }
            catch (InvalidOperationException ex)
            {
                var errorMessage = "An invalid operation occurred. Please check your request.";
                errorMessage += $" Details: {ex.Message}";
                return BadRequest(new { message = errorMessage });
            }
            catch (Exception ex)
            {
                var errorMessage = "An unexpected error occurred while processing your request.";
                errorMessage += $" Details: {ex.Message}";
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = errorMessage });
            }

        }

        [HttpGet("GetProduct/{id}")]
        public async Task<IActionResult> GetProduct(int id)
        {
            try
            {
                var product = await _mediator.Send(new GetProductQuery(id));
                return Ok(product);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(ex.Message);
            }
            catch (Exception ex)
            {
                var errorMessage = "An unexpected error occurred while processing your request.";
                errorMessage += $" Details: {ex.Message}";
                return StatusCode(StatusCodes.Status500InternalServerError, new { message = errorMessage });
            }
        }
    }
}