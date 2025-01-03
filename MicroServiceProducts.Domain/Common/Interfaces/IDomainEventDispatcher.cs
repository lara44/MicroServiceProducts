

using Domain.Common.Models;

namespace Domain.Common.Interfaces
{
    public interface IDomainEventDispatcher
    {
        Task DispatchEventsAsync(AggregateRoot entity);
    }
}