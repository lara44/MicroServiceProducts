

using Domain.ProductAggregate;

namespace Domain.Common.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task DispatchEventsAsync(AggregateRoot entity);
    }
}