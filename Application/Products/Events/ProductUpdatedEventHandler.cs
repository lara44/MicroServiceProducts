
// using Application.Common.Interfaces;
// using Domain.Product.Events;
// using MediatR;
// using Microsoft.Extensions.Logging;

// namespace Application.Products.Events
// {
//     public class ProductUpdatedEventHandler : INotificationHandler<ProductUpdatedEvent>
//     {
//         private const string EventType = "ProductUpdated";
//         private readonly IEventPublisher _eventPublisher;
//         private readonly ILogger<ProductUpdatedEventHandler> _logger;


//         public ProductUpdatedEventHandler(
//             IEventPublisher eventPublisher,
//             ILogger<ProductUpdatedEventHandler> logger
//         )
//         {
//             _eventPublisher = eventPublisher;
//             _logger = logger;
//         }

//         public async Task Handle(ProductUpdatedEvent notification, CancellationToken cancellationToken)
//         {
//             var queueName = "ProductEventsQueue";
//             var message = new
//             {
//                 Id = notification.Product.Id,
//                 Name = notification.Product.Name,
//                 Price = notification.Product.Price
//             };

//             _logger.LogInformation($"Publishing ProductUpdated event for Product ID: {notification.Product.Id}");
//             await _eventPublisher.PublishAsync(message, queueName, EventType, cancellationToken);
//             _logger.LogInformation($"Published ProductUpdated event for Product ID: {notification.Product.Id}");
//         }
//     }
// }