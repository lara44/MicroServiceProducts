
using Application.Common.Interfaces;
using Application.Products.Services.Interfaces;
using Domain.Entities;

namespace Application.Products.Services
{
    public class ProductEventService : IProductEventService
    {
        private readonly IEventPublisher _eventPublisher;

        public ProductEventService(
            IEventPublisher eventPublisher
        )
        {
            _eventPublisher = eventPublisher;
        }

        public async Task PublishProductCreatedEventAsync(Product product, string eventType, CancellationToken cancellationToken)
        {
            var queueName = "ProductEventsQueue";
            var message = new
            {
                Id = product.Id,
                Name = product.Name,
                Price = product.Price
            };

            await _eventPublisher.PublishAsync(message, queueName, eventType, cancellationToken);
        }
    }
}