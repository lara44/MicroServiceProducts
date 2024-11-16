
using Application.Common.Interfaces;
using Domain.Product.Events;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Products.Events
{
    public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
    {
        private const string EventType = "ProductCreated";
        private readonly IEventPublisher _eventPublisher;
        ILogger<ProductCreatedEventHandler> _logger;

        public ProductCreatedEventHandler(
            IEventPublisher eventPublisher,
            ILogger<ProductCreatedEventHandler> logger
        )
        {
            _eventPublisher = eventPublisher;
            _logger = logger;
        }

        public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
        {
            var queueName = "ProductEventsQueue";
            var message = new
            {
                Id = notification.Product.Id,
                Name = notification.Product.Name,
                Price = notification.Product.Price
            };

            _logger.LogInformation($"Publishing ProductCreated event for Product ID: {notification.Product.Id}");
            await _eventPublisher.PublishAsync(message, queueName, EventType, cancellationToken);
            _logger.LogInformation($"Published ProductCreated event for Product ID: {notification.Product.Id}");
        }
    }
}