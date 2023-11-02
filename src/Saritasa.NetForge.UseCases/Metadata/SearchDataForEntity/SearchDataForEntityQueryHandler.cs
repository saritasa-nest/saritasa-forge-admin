using MediatR;
using Saritasa.NetForge.DomainServices.Extensions;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.NetForge.UseCases.Common;
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

        query = Search(query, searchOptions, request);

        var pagedList = PagedListFactory.FromSource(query, searchOptions.Page, searchOptions.PageSize);

        return Task.FromResult(pagedList.ToMetadataObject());
    }

    private IQueryable<object> Search(
        IQueryable<object> query, SearchOptions searchOptions, SearchDataForEntityQuery request)
    {
        if (!string.IsNullOrEmpty(searchOptions.SearchString))
        {
            if (request.Properties.Any(property => property.SearchType is not null))
            {
                query = dataService
                    .Search(query, searchOptions.SearchString, request.EntityType!, request.Properties);
            }

            if (request.SearchFunction is not null)
            {
                query = request.SearchFunction(null, query, searchOptions.SearchString);
            }
        }

        return query;
    }
}
