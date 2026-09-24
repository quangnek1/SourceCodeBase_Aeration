using AerationSterilize.Application.Features.V1.AerationPosition.Common.Dtos;
using Contracts.Common.Messages;
using Shared.Emumerations;
using Shared.Paging;

namespace AerationSterilize.Application.Features.V1.AerationPosition.Queries.GetAerationPositions;

public record GetAerationPositionsQuery(
    string? SearchTerm,
    string? SortColumn,
    SortOrder? SortOrder,
    int PageIndex,
    int PageSize) : IQuery<PagedResult<AerationPositionDto>>;
