
using MediatR;

namespace Domain.Common.Models;

public class AggregateRoot
{
        private readonly List<INotification> _domainEvents = new List<INotification>();
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        // Método para agregar eventos de dominio
        protected void AddDomainEvent(INotification eventItem)     
        {
            if (!_domainEvents.Contains(eventItem))
            {
                Console.WriteLine("Adding event: " + eventItem.GetType().Name);
                _domainEvents.Add(eventItem);
            }
        }

        // Método para limpiar los eventos
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
}
