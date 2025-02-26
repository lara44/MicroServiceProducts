using Domain.Common.Interfaces;
using Domain.Product;
using Domain.Product.Repositories;
using MediatR;
using MicroServiceProducts.Application.Products.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Application.Products.Commands.UpdateProduct
{
    public class UpdateProductCommandHandler : IRequestHandler<UpdateProductCommand>
    {
        private readonly ICategoryValidator _categoryValidator;
        private readonly IProductRepository _productRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly ILogger<UpdateProductCommandHandler> _logger;

        public UpdateProductCommandHandler(
            ICategoryValidator categoryValidator,
            IProductRepository productRepository,
            IDomainEventDispatcher domainEventDispatcher,
            ILogger<UpdateProductCommandHandler> logger
        )
        {
            _categoryValidator = categoryValidator;
            _productRepository = productRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _logger = logger;
        }
        public async Task<Unit> Handle(UpdateProductCommand request, CancellationToken cancellationToken)
        {
            var product = await _productRepository.GetByIdAsync(request.Id);

            if (product == null)
            {
                throw new KeyNotFoundException($"El producto con el ID {request.Id} no fue encontrado.");
            }

            var price = Price.Create(request.Price);
            product.Update(request.Name!, price, request.Stock);

            var categories = await _categoryValidator.ValidateCategories(request.CategoryIds);

            foreach (var category in categories)
            {
                if (!product.Categories.Any(c => c.Id == category.Id)) // Verificar si ya existe
                {
                    product.AddCategory(category);
                }
            }

            await _productRepository.UpdateAsync(product);

            _logger.LogInformation("Despachar Evento de dominio ProductUpdateEvent para el producto {ProductId}", product.Id);
            await _domainEventDispatcher.DispatchEventsAsync(product);
            return Unit.Value;
        }
    }
}