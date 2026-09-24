using System.Linq.Expressions;
using AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
using AerationSterilize.Application.Features.V1.Batch.Common.Extensions;
using AerationSterilize.Domain.Entities;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using Shared.Paging;

namespace AerationSterilize.Application.Features.V1.Batch.Queries.GetItemsProgress;
internal class GetItemsProgressQueryHandler : IQueryHandler<GetItemsProgressQuery, PagedResult<ItemProgessDto>>
{
    private readonly IRepositoryBase<Domain.Entities.Batch, int> _batchRepository;
    private readonly IRepositoryBase<BatchItem, int> _batchItemRepository;
    private readonly IMapper _mapper;

    public GetItemsProgressQueryHandler(IRepositoryBase<Domain.Entities.Batch, int> batchRepository,
        IMapper mapper,
        IRepositoryBase<BatchItem, int> batchItemRepository)
    {
        _batchRepository = batchRepository ?? throw new ArgumentNullException(nameof(batchRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _batchItemRepository = batchItemRepository ?? throw new ArgumentNullException(nameof(batchItemRepository));
    }

    public async Task<Result<PagedResult<ItemProgessDto>>> Handle(GetItemsProgressQuery request, CancellationToken cancellationToken)
    {
        var lamda = new Func<IQueryable<BatchItem>, IQueryable<BatchItem>>(q =>
        {
            return q.Include(x => x.PackingPosition)
                    .Include(x => x.Batch)
                    .ThenInclude(x => x.BatchInAerationPositions)
                    .ThenInclude(x => x.AerationPosition)
                    .Include(x => x.Boxes)
                    .ThenInclude(x=>x.ItemTags)
                    ;
        });

        Expression<Func<BatchItem, bool>> lambdaSearch = x
            => x.Batch.BatchNo.Contains(request.searchTerm) || x.InternalLot.Contains(request.searchTerm);

        var batchesQuery = string.IsNullOrWhiteSpace(request.searchTerm)
          ? _batchItemRepository.FindAll(null, false, lamda)// Search = null, include = lamda
          : _batchItemRepository.FindAll(lambdaSearch, false, lamda); // Search = lambdaSearch, include = lamda

        var sortParts = request.sort?.Split(',', StringSplitOptions.RemoveEmptyEntries) ?? [];
        IOrderedQueryable<BatchItem>? orderedQuery = null;

        foreach (var part in sortParts)
        {
            var segments = part.Trim().Split(':');
            var col = segments[0];
            var desc = segments.Length > 1 && segments[1].ToLower() == "desc";
            var expr = BatchItemExtension.GetSortExpression(col);

            orderedQuery = orderedQuery == null
                ? (desc ? batchesQuery.OrderByDescending(expr) : batchesQuery.OrderBy(expr))
                : (desc ? orderedQuery.ThenByDescending(expr) : orderedQuery.ThenBy(expr));
        }

        batchesQuery = orderedQuery ?? batchesQuery.OrderBy(x => x.Id);

        var pagedResult = await PagingExtensions.ToPagedResultAsync(
                  batchesQuery,
                  request.PageIndex,
                  request.PageSize);

        var result = _mapper.Map<PagedResult<ItemProgessDto>>(pagedResult);

        return Result<PagedResult<ItemProgessDto>>.Success(result);
    }
}
