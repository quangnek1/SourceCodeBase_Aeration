using AerationSterilize.Application.Features.V1.AerationColumn.Common.Dtos;
using AerationSterilize.Application.Features.V1.AerationColumn.Common.Extensions;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Shared.Emumerations;
using Shared.Paging;

namespace AerationSterilize.Application.Features.V1.AerationColumn.Queries.GetAerationColumns;

public class GetAerationColumnsQueryHandler : IQueryHandler<GetAerationColumnsQuery, PagedResult<AerationColumnDto>>
{
    private readonly IRepositoryBase<Domain.Entities.AerationColumn, int> _repository;
    private readonly IMapper _mapper;

    public GetAerationColumnsQueryHandler(
        IRepositoryBase<Domain.Entities.AerationColumn, int> repository,
        IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<PagedResult<AerationColumnDto>>> Handle(GetAerationColumnsQuery request, CancellationToken cancellationToken)
    {
        var query = string.IsNullOrWhiteSpace(request.SearchTerm)
            ? _repository.FindAll()
            : _repository.FindAll(x => x.ColumnName.Contains(request.SearchTerm));

        var sortExpression = AerationColumnExtension.GetSortExpression(request.SortColumn);

        query = request.SortOrder == SortOrder.Descending
            ? query.OrderByDescending(sortExpression)
            : query.OrderBy(sortExpression);

        var pagedResult = await PagingExtensions.ToPagedResultAsync(query, request.PageIndex, request.PageSize);

        var result = _mapper.Map<PagedResult<AerationColumnDto>>(pagedResult);

        return Result.Success(result);
    }
}
