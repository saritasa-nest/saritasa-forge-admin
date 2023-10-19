using MediatR;
using Saritasa.NetForge.Infrastructure.Abstractions.Interfaces;
using Saritasa.Tools.Common.Pagination;

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
        var data = dataService.GetData(request.EntityType).OfType<object>().ToList();

        var searchOptions = request.SearchOptions;
        var pagedList = PagedListFactory.FromSource(data, searchOptions.Page, searchOptions.PageSize);

        return Task.FromResult(pagedList.ToMetadataObject());
    }
}
