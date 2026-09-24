using AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Contracts.Services.QRCode;
using Microsoft.EntityFrameworkCore;

namespace AerationSterilize.Application.Features.V1.Batch.Queries.GetBatchF6112;
internal class GetBatchByQrcodeQueryHandler : IQueryHandler<GetBatchByQrcodeQuery, BatchScanQrcodeDto>
{
    private readonly IRepositoryBase<Domain.Entities.Batch, int> _batchRepository;
    private readonly IMapper _mapper;

    public GetBatchByQrcodeQueryHandler(IRepositoryBase<Domain.Entities.Batch, int> batchRepository, IMapper mapper)
    {
        _batchRepository = batchRepository ?? throw new ArgumentNullException(nameof(batchRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<BatchScanQrcodeDto>> Handle(GetBatchByQrcodeQuery request, CancellationToken cancellationToken)
    {
        var lamda = new Func<IQueryable<Domain.Entities.Batch>, IQueryable<Domain.Entities.Batch>>(q =>
        {
            return q.Include(x=>x.BatchInAerationPositions)
                    .ThenInclude(x=>x.AerationPosition)
                    .Include(x => x.BatchItems)
                    ;
        });

        var batch = await _batchRepository
                .FindAll(x => x.QRCode.Equals(request.Qrcode), false, lamda)
                .SingleOrDefaultAsync(cancellationToken);

        if (batch == null)
        {
            return Result<BatchScanQrcodeDto>.Failure<BatchScanQrcodeDto>(new Error("404", "Batch Not Found"));
        }

        var result = _mapper.Map<BatchScanQrcodeDto>(batch);

        return Result<BatchScanQrcodeDto>.Success(result);
    }
}
