using Domain.Common.Interfaces;
using Domain.Common.Models;
using MassTransit;
using Microsoft.Extensions.Logging;

namespace Application.Products.Services;

public class DomainEventDispatcher : IDomainEventDispatcher
{
    private readonly IPublishEndpoint _publishEndpoint;
    private readonly ILogger<DomainEventDispatcher> _logger;

    public DomainEventDispatcher(
        IPublishEndpoint publishEndpoint,
        ILogger<DomainEventDispatcher> logger)
    {
        _publishEndpoint = publishEndpoint;
        _logger = logger;
    }

    public async Task DispatchEventsAsync(AggregateRoot aggregateRoot)
    {
        var domainEvents = aggregateRoot.DomainEvents.ToList();
        aggregateRoot.ClearDomainEvents();

        foreach (var domainEvent in domainEvents)
        {
            try
            {
                _logger.LogInformation($"Publishing event: {domainEvent.GetType().Name}");
                await _publishEndpoint.Publish(domainEvent);
                
                _logger.LogInformation($"Event {domainEvent.GetType().Name} published successfully.");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, $"Error publishing event: {domainEvent.GetType().Name}");
            }
        }
    }
}
