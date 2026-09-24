using AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
using Contracts.Common.Messages;
using Shared.Emumerations;
using Shared.Paging;

namespace AerationSterilize.Application.Features.V1.Batch.Queries.GetAllBatchF6112;
public sealed record GetAllBatchF6112Query(string? searchTerm,
    string? sortColumn,
    SortOrder? SortOrder,
    int PageIndex, int PageSize) : IQuery<PagedResult<BatchDto>>;
