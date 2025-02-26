
using MediatR;
using Domain.Common.Interfaces;
using Microsoft.Extensions.Logging;
using Domain.Product.Repositories;
using Domain.Product;
using MicroServiceProducts.Application.Products.Services;
using MicroServiceProducts.Application.Products.Services.Interfaces;

namespace Application.Products.Commands.CreateProduct
{
    public class CreateProductCommandHandler : IRequestHandler<CreateProductCommand, Guid>
    {
        private readonly ICategoryValidator _categoryValidator;
        private readonly IProductRepository _productRepository;
        private readonly IDomainEventDispatcher _domainEventDispatcher;
        private readonly ILogger<CreateProductCommandHandler> _logger;

        public CreateProductCommandHandler(
            ICategoryValidator categoryValidator,
            IProductRepository productRepository,
            IDomainEventDispatcher domainEventDispatcher,
            ILogger<CreateProductCommandHandler> logger
        )
        {
            _categoryValidator = categoryValidator;
            _productRepository = productRepository;
            _domainEventDispatcher = domainEventDispatcher;
            _logger = logger;
        }

        public async Task<Guid> Handle(CreateProductCommand request, CancellationToken cancellationToken)
        {
            _logger.LogInformation("Inicio del proceso de creación del producto: {ProductName}, Precio: {Price}, Stock: {Stock}",
            request.Name, request.Price, request.Stock);

            var price = Price.Create(request.Price);
            var product = Product.Create(request.Name, price, request.Stock);

            var categories = await _categoryValidator.ValidateCategories(request.CategoryIds);

            foreach (var category in categories)
            {
                product.AddCategory(category);
            }

            await _productRepository.AddAsync(product);
            _logger.LogInformation("Producto agregado al repositorio: Id {ProductId}", product.Id);

            _logger.LogInformation("Eventos de dominio despachados para el producto Id {ProductId}", product.Id);
            await _domainEventDispatcher.DispatchEventsAsync(product);
            _logger.LogInformation("Proceso de creación del producto completado: Id {ProductId}", product.Id);

            return product.Id;
        }
    }
}