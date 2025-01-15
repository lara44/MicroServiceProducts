
using Domain.Category;
using Domain.Category.Respositories;
using Domain.Common.Interfaces;
using MediatR;
using Microsoft.Extensions.Logging;

namespace Application.Categories.Commands.CreateCategory;

public class CreateCategoryHandler : IRequestHandler<CreateCategoryCommand, Guid>
{
    private readonly ICategoryRepository _categoryRepository;
    private readonly IDomainEventDispatcher _domainEventDispatcher;
    private readonly ILogger<CreateCategoryHandler> _logger;

    public CreateCategoryHandler(
        ICategoryRepository categoryRepository,
        IDomainEventDispatcher domainEventDispatcher,
        ILogger<CreateCategoryHandler> logger
    )
    {
        _categoryRepository = categoryRepository;
        _domainEventDispatcher = domainEventDispatcher;
        _logger = logger;
    }

    public async Task<Guid> Handle(CreateCategoryCommand request, CancellationToken cancellationToken)
    {
        _logger.LogInformation("Inicia proceso de creación categoria: {Name}", request.Name);
        var category = Category.Create(request.Name!);

        await _categoryRepository.AddSync(category);
        _logger.LogInformation("Categoria agregada al repositorio: Id {CategoryId}", category.Id);
    
        _logger.LogInformation("Eventos de dominio despachados para la categoria Id {CategoryId}", category.Id);
        await _domainEventDispatcher.DispatchEventsAsync(category);
        _logger.LogInformation("Proceso de creación de la categoria completado: Id {CategoryId}", category.Id);
        return category.Id;
    }
}
