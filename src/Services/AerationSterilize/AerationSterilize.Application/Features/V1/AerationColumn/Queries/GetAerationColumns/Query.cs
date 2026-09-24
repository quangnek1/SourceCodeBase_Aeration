using AerationSterilize.Application.Features.V1.AerationColumn.Common.Dtos;
using Contracts.Common.Messages;
using Shared.Emumerations;
using Shared.Paging;

namespace AerationSterilize.Application.Features.V1.AerationColumn.Queries.GetAerationColumns;

public record GetAerationColumnsQuery(
    string? SearchTerm,
    string? SortColumn,
    SortOrder? SortOrder,
    int PageIndex,
    int PageSize) : IQuery<PagedResult<AerationColumnDto>>;
