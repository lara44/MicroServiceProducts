
namespace Application.Common.Interfaces
{
    public interface IEventPublisher
    {
        Task PublishAsync<T>(T @event, string queueUrl, string eventType, CancellationToken cancellationToken = default);
    }
}