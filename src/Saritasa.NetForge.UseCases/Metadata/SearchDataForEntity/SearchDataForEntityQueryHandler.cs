using System.Linq;
using System.Linq.Expressions;
using System.Reflection;
using MediatR;
using Saritasa.NetForge.Domain.Attributes;
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
                    //query = query.Where(entity => EF.Functions.ILike(entity.Name, searchOptions.SearchString));

                    // ---

                    //var searchConstant = Expression.Constant(searchOptions.SearchString);
                    //var entityParam = Expression.Parameter(typeof(object), "entity");
                    //var converted = Expression.Convert(entityParam, request.EntityType);
                    //var propertyExpression = Expression.Property(converted, property.Name);

                    //var isMatchMethod = typeof(System.Text.RegularExpressions.Regex).GetMethod("IsMatch",
                    //    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                    //    null,
                    //    new[]
                    //    {
                    //        typeof(string),
                    //        typeof(string),
                    //        typeof(System.Text.RegularExpressions.RegexOptions)
                    //    },
                    //    null
                    //);

                    //var isMatchCall = Expression.Call(isMatchMethod, propertyExpression, searchConstant,
                    //    Expression.Constant(System.Text.RegularExpressions.RegexOptions.IgnoreCase));

                    //var predicate = Expression.Lambda<Func<object, bool>>(isMatchCall, entityParam);

                    // ---

                    //var iLike = typeof(NpgsqlDbFunctionsExtensions).GetMethod("ILike",
                    //    BindingFlags.Static | BindingFlags.Public | BindingFlags.NonPublic,
                    //    null,
                    //    new[] {
                    //        typeof(Microsoft.EntityFrameworkCore.DbFunctions),
                    //        typeof(string),
                    //        typeof(string)
                    //    },
                    //    null
                    //);

                    //var iLikeCall = Expression.Call(
                    //    iLike,
                    //    Expression.Constant(null, typeof(DbFunctions)),
                    //    property,
                    //    Expression.Constant(search, typeof(string)));

                    query = query.Where(predicate);
                }
            }
        }

        var pagedList = PagedListFactory.FromSource(query, searchOptions.Page, searchOptions.PageSize);

        return Task.FromResult(pagedList.ToMetadataObject());
    }
}
