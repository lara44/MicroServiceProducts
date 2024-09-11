
using Application.Products.Services.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Products.Events
{
    public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
    {
        private readonly IProductEventService _productEventService;

        public ProductCreatedEventHandler(IProductEventService productEventService)
        {
            _productEventService = productEventService;
        }

        public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            await _productEventService.PublishProductCreatedEventAsync(notification.Product, "ProductCreated", cancellationToken);
        }
    }
}