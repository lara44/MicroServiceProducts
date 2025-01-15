using Domain.Product.Events;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Application.Products.Events;

public class ProductCreatedEventHandler
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<ProductCreatedEventHandler> _logger;

    public ProductCreatedEventHandler(
        IPublishEndpoint publishEndpoint,
        ILogger<ProductCreatedEventHandler> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task Handle(ProductCreatedEvent notification, CancellationToken cancellationToken)
    {
        _logger.LogInformation($"Publishing ProductCreated event for Product ID: {notification.Id}");

        try
        {
            // Publica el evento directamente usando MassTransit
            await _publishEndpoint.Publish(notification, cancellationToken);

            _logger.LogInformation($"Successfully published ProductCreated event for Product ID: {notification.Id}");
        }
        catch (Exception ex)
        {
            _logger.LogError(ex, $"Error publishing ProductCreated event for Product ID: {notification.Id}");
        }
    }
}
