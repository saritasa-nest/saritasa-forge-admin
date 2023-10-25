using System.Linq;
using System.Linq.Expressions;
using MediatR;
using Saritasa.NetForge.DomainServices.Extensions;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.Tools.Common.Extensions;
using Saritasa.Tools.Common.Pagination;
using Saritasa.Tools.Domain.Exceptions;

namespace Saritasa.NetForge.UseCases.Metadata.SearchDataForEntity;

/// <summary>
/// Handler for <see cref="SearchDataForEntityQuery"/>.
/// </summary>
internal class SearchDataForEntityQueryHandler : IRequestHandler<SearchDataForEntityQuery, PagedListMetadataDto<object>>
{
    private readonly IOrmDataService dataService;

    /// <summary>
    /// Constructor.
    /// </summary>
    public SearchDataForEntityQueryHandler(IOrmDataService dataService)
    {
        this.dataService = dataService;
    }

    /// <inheritdoc/>
    public Task<PagedListMetadataDto<object>> Handle(
        SearchDataForEntityQuery request, CancellationToken cancellationToken)
    {
        if (request.EntityType is null)
        {
            throw new NotFoundException("Entity with given type was not found.");
        }

        var query = dataService.GetQuery(request.EntityType);

        query = query.SelectProperties(request.EntityType, request.Properties);

        var searchOptions = request.SearchOptions;

        if (!string.IsNullOrEmpty(searchOptions.SearchString))
        {
            foreach (var property in request.Properties)
            {
                if (property.IsSearchable)
                {
                    var searchConstant = Expression.Constant(searchOptions.SearchString);
                    var entityParam = Expression.Parameter(typeof(object), "entity");
                    var converted = Expression.Convert(entityParam, request.EntityType);
                    var propertyExpression = Expression.Property(converted, property.Name);
                    var body = Expression.Equal(propertyExpression, searchConstant);

                    var predicate = Expression.Lambda<Func<object, bool>>(body, entityParam);

                    query = query.Where(predicate);
                }
            }
        }

        var pagedList = PagedListFactory.FromSource(query, searchOptions.Page, searchOptions.PageSize);

        return Task.FromResult(pagedList.ToMetadataObject());
    }

    private void Sort()
    {
        
    }
}
