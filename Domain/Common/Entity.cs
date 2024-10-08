
using MediatR;

namespace Domain.Common
{
    public abstract class Entity
    {
        private readonly List<INotification> _domainEvents = new List<INotification>();
        public IReadOnlyCollection<INotification> DomainEvents => _domainEvents.AsReadOnly();

        // Método para agregar eventos de dominio
        protected void AddDomainEvent(INotification eventItem)
        {
            _domainEvents.Add(eventItem);
        }

        // Método para limpiar los eventos
        public void ClearDomainEvents()
        {
            _domainEvents.Clear();
        }
    }
}