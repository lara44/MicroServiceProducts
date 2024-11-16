

using Domain.Common.Interfaces;
using Domain.Common.Models;
using MediatR;

namespace Application.Products.Services
{
    public class DomainEventDispatcher : IDomainEventDispatcher
    {
        private readonly IMediator _mediator;

        public DomainEventDispatcher(IMediator mediator)
        {
            _mediator = mediator;
        }

        public async Task DispatchEventsAsync(AggregateRoot aggregateRoot)
        {
            var domainEvents = aggregateRoot.DomainEvents.ToList();

            // Limpiar los eventos después de despacharlos
            aggregateRoot.ClearDomainEvents();

            foreach (var domainEvent in domainEvents)
            {
                // Publicar evento a través de MediatR
                await _mediator.Publish(domainEvent);
            }
        }
    }
}