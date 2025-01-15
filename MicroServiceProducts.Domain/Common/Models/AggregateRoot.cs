
using System.Text.Json.Serialization;
using MediatR;

namespace Domain.Common.Models;

public class AggregateRoot : Entity
{
    private readonly List<object> _domainEvents = new List<object>();

    [JsonIgnore]
    public IReadOnlyCollection<object> DomainEvents => _domainEvents.AsReadOnly();

    protected AggregateRoot(Guid id) : base(id) { }

    public void AddDomainEvent(object eventItem)
    {
        _domainEvents.Add(eventItem);
    }

    public void ClearDomainEvents()
    {
        _domainEvents.Clear();
    }
}
