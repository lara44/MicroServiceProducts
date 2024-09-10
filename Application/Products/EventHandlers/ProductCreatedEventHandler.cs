
using Application.Products.Services.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Products.EventHandlers
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
            // Publicar el evento en SQS a través de IProductEventService
            await _productEventService.PublishProductCreatedEventAsync(notification.Product, cancellationToken);
        }
    }
}