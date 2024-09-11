
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

        [HttpPut("UpdateProduct")]
        public async Task<IActionResult> UpdateProduct(Guid id, [FromBody] UpdateProductCommand command)
        {
            try
            {
                if (command == null || id != command.Id)
                {
                    return BadRequest("Invalid product data or ID mismatch");
                }

                await _mediator.Send(command);
                return NoContent();
            }
            catch (KeyNotFoundException ex)
            {
                // Retornar 404 si no se encuentra el producto
                return NotFound(new { Message = "Producto con ID {id} no encontrado", Detail = ex.Message });
            }
            catch (Exception ex)
            {
                // Retornar error 500 si ocurre una excepción no manejada
                return StatusCode(500, new { Code = 404, Message = "Ocurrió un error al actualizar el producto", Detail = ex.Message });
            }
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
        public async Task<IActionResult> GetProduct(Guid id)
        {
            try
            {
                var product = await _mediator.Send(new GetProductByIdQuery(id));
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

        [HttpGet("GetProducts")]
        public async Task<IActionResult> GetProducts()
        {
            try
            {
                var products = await _mediator.Send(new GetAllProductsQuery());
                return Ok(products);
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