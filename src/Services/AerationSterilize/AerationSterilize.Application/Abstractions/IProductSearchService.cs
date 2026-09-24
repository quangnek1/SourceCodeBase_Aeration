using AerationSterilize.Application.Features.V1.Products.Common.Dtos;
using Shared.Emumerations;
using Shared.Paging;

namespace AerationSterilize.Application.Abstractions;

public interface IProductSearchService
{
    bool IsEnabled { get; }

    Task<PagedResult<ProductDto>> SearchAsync(
        string? searchTerm,
        string? sortColumn,
        SortOrder? sortOrder,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default);
}