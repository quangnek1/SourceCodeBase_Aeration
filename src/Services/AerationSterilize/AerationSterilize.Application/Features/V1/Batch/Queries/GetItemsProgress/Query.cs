using AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
using Contracts.Common.Messages;
using Shared.Paging;

namespace AerationSterilize.Application.Features.V1.Batch.Queries.GetItemsProgress;
public sealed record GetItemsProgressQuery(
    string? searchTerm,
    string? sort,
    int PageIndex, int PageSize) : IQuery<PagedResult<ItemProgessDto>>;

