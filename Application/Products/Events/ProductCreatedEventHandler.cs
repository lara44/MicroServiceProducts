
using Application.Common.Interfaces;
using Domain.Events;
using MediatR;

namespace Application.Products.Events
{
    public class ProductCreatedEventHandler : INotificationHandler<ProductCreatedEvent>
    {
        private const string EventType = "ProductCreated";
        private readonly IEventPublisher _eventPublisher;

        public ProductCreatedEventHandler(IEventPublisher eventPublisher)
        {
            _eventPublisher = eventPublisher;
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

            Console.WriteLine($"Publishing ProductCreated event for Product ID: {notification.Product.Id}");
            await _eventPublisher.PublishAsync(message, queueName, EventType, cancellationToken);
            Console.WriteLine($"Published ProductCreated event for Product ID: {notification.Product.Id}");
        }
    }
}