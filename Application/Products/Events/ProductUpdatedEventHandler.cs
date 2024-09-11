
using Application.Products.Services.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Products.Events
{
    public class ProductUpdatedEventHandler : INotificationHandler<ProductUpdatedEvent>
    {
        private const string EventType = "ProductUpdated";
        private readonly IProductEventService _productEventService;

        public ProductUpdatedEventHandler(IProductEventService productEventService)
        {
            _productEventService = productEventService;
        }

        public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
        {
            await _productEventService.PublishProductCreatedEventAsync(notification.Product, EventType, cancellationToken);
        }
    }
}