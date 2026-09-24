using AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
using AerationSterilize.Application.Features.V1.Batch.Common.Extensions;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using MediatR;
using Shared.Emumerations;
using Shared.Paging;

namespace AerationSterilize.Application.Features.V1.Batch.Queries.GetBatchPrintPDF;
internal class GetBatchPrintPDFQueryHandler : IQueryHandler<GetBatchPrintPDFQuery, PagedResult<BatchPrintPdfDto>>
{
    private readonly IRepositoryBase<Domain.Entities.Batch, int> _batchRepository;
    private readonly IPublisher _publisher;
    private readonly IMapper _mapper;

    public GetBatchPrintPDFQueryHandler(IRepositoryBase<Domain.Entities.Batch, int> batchRepository, IPublisher publisher, IMapper mapper)
    {
        _batchRepository = batchRepository ?? throw new ArgumentNullException(nameof(batchRepository));
        _publisher = publisher ?? throw new ArgumentNullException(nameof(publisher));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }
    public async Task<Result<PagedResult<BatchPrintPdfDto>>> Handle(GetBatchPrintPDFQuery request, CancellationToken cancellationToken)
    {
        var batchesQuery = _batchRepository.FindAll(null, false);

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

        var result = _mapper.Map<PagedResult<BatchPrintPdfDto>>(pagedResult);

        return Result<PagedResult<BatchPrintPdfDto>>.Success(result);
    }
}
