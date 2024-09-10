
using Domain.Common;
using Domain.Common.Interfaces;
using Domain.Entities;
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

        public async Task DispatchEventsAsync(Entity entity)
        {
            var domainEvents = entity.DomainEvents.ToList();
            entity.ClearDomainEvents(); // Limpiar los eventos después de despacharlos

            foreach (var domainEvent in domainEvents)
            {
                await _mediator.Publish(domainEvent);  // Publicar evento a través de MediatR
            }
        }

        public Task DispatchEventsAsync(Product product)
        {
            throw new NotImplementedException();
        }
    }
}