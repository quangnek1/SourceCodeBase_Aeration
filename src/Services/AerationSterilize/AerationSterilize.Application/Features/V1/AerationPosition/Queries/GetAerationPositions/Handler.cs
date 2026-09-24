using AerationSterilize.Application.Features.V1.AerationPosition.Common.Dtos;
using AerationSterilize.Application.Features.V1.AerationPosition.Common.Extensions;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Shared.Emumerations;
using Shared.Paging;

namespace AerationSterilize.Application.Features.V1.AerationPosition.Queries.GetAerationPositions;

public class GetAerationPositionsQueryHandler : IQueryHandler<GetAerationPositionsQuery, PagedResult<AerationPositionDto>>
{
    private readonly IRepositoryBase<Domain.Entities.AerationPosition, int> _repository;
    private readonly IMapper _mapper;

    public GetAerationPositionsQueryHandler(
        IRepositoryBase<Domain.Entities.AerationPosition, int> repository,
        IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<PagedResult<AerationPositionDto>>> Handle(GetAerationPositionsQuery request, CancellationToken cancellationToken)
    {
        var query = string.IsNullOrWhiteSpace(request.SearchTerm)
            ? _repository.FindAll()
            : _repository.FindAll(x => x.PositionCode.Contains(request.SearchTerm));

        var sortExpression = AerationPositionExtension.GetSortExpression(request.SortColumn);

        query = request.SortOrder == SortOrder.Descending
            ? query.OrderByDescending(sortExpression)
            : query.OrderBy(sortExpression);

        var pagedResult = await PagingExtensions.ToPagedResultAsync(query, request.PageIndex, request.PageSize);

        var result = _mapper.Map<PagedResult<AerationPositionDto>>(pagedResult);

        return Result.Success(result);
    }
}
