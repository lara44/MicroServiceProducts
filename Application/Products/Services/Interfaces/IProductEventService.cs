
using Domain.Entities;

namespace Application.Products.Services.Interfaces
{
    public interface IProductEventService
    {
        Task PublishProductCreatedEventAsync(Product product, string eventType, CancellationToken cancellationToken);
    }
}