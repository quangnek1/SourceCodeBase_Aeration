using AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
using AerationSterilize.Application.Features.V1.Batch.Common.Extensions;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using Shared.Emumerations;
using Shared.Paging;

namespace AerationSterilize.Application.Features.V1.Batch.Queries.GetAllBatchF6112;
internal class GetAllBatchF6112QueryHandler : IQueryHandler<GetAllBatchF6112Query, PagedResult<BatchDto>>
{
    private readonly IRepositoryBase<Domain.Entities.Batch, int> _batchRepository;
    private readonly IMapper _mapper;

    public GetAllBatchF6112QueryHandler(IRepositoryBase<Domain.Entities.Batch, int> batchRepository, IMapper mapper)
    {
        _batchRepository = batchRepository ?? throw new ArgumentNullException(nameof(batchRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<PagedResult<BatchDto>>> Handle(GetAllBatchF6112Query request, CancellationToken cancellationToken)
    {
        var batchesQuery = _batchRepository.FindAll(null, false,
            q => q
            .Include(x => x.BatchInAerationPositions)
            .ThenInclude(x => x.AerationPosition));

        if (!string.IsNullOrWhiteSpace(request.searchTerm))
        {
            batchesQuery = batchesQuery.Where(x => x.BatchNo.Contains(request.searchTerm));
        }

        var sortExpression = BatchExtension.GetSortExpression(request.sortColumn);

        batchesQuery = request.SortOrder == SortOrder.Descending
        ? batchesQuery.OrderByDescending(sortExpression)
        : batchesQuery.OrderBy(sortExpression);

        var pagedResult = await PagingExtensions.ToPagedResultAsync(
                  batchesQuery,
                  request.PageIndex,
                  request.PageSize);

        var result = _mapper.Map<PagedResult<BatchDto>>(pagedResult);

        return Result<PagedResult<BatchDto>>.Success(result);
    }
}
