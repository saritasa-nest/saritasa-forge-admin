using System.Linq;
using System.Linq.Expressions;
using MediatR;
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

        var data = dataService.GetData(request.EntityType).OfType<object>();

        var searchOptions = request.SearchOptions;

        if (searchOptions.SearchString is not null)
        {
            foreach (var property in request.Properties)
            {
                if (property.IsSearchable)
                {
                    data = data
                        .Where(d =>
                                 d
                                .GetType()
                                .GetProperty(property.Name)
                                .GetValue(d)
                                .ToString()
                                .Equals(searchOptions.SearchString));

                    try
                    {
                        var searchConstant = Expression.Constant(searchOptions.SearchString);
                        var entityParam = Expression.Parameter(request.EntityType);
                        var propertyExpression = Expression.Property(entityParam, property.Name);
                        var body = Expression.Equal(propertyExpression, searchConstant);
                        var predicate = Expression.Lambda<Func<object, bool>>(body, entityParam);

                        data = data.Where(predicate);
                    }
                    catch (Exception ex)
                    {

                    }

                }
            }
        }

        var pagedList = PagedListFactory.FromSource(data, searchOptions.Page, searchOptions.PageSize);

        return Task.FromResult(pagedList.ToMetadataObject());
    }

    private void Sort()
    {
        
    }
}
