using AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Contracts.Services.QRCode;
using Microsoft.EntityFrameworkCore;

namespace AerationSterilize.Application.Features.V1.Batch.Queries.GetBatchF6112;
internal class GetBatchF6112QueryHandler : IQueryHandler<GetBatchByIdQuery, BatchDto>
{
    private readonly IRepositoryBase<Domain.Entities.Batch, int> _batchRepository;
    private readonly IMapper _mapper;
    private readonly IQrCodeServices _qrCodeServices;

    public GetBatchF6112QueryHandler(IRepositoryBase<Domain.Entities.Batch, int> batchRepository, IMapper mapper,
        IQrCodeServices qrCodeServices)
    {
        _batchRepository = batchRepository ?? throw new ArgumentNullException(nameof(batchRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _qrCodeServices = qrCodeServices ?? throw new ArgumentNullException(nameof(qrCodeServices));
    }

    public async Task<Result<BatchDto>> Handle(GetBatchByIdQuery request, CancellationToken cancellationToken)
    {
        //var batch = await _batchRepository
        //    .FindSingleAsync(x => x.Id == request.Id, cancellationToken,
        //    p => p.BatchItems, p => p.BatchInAerationPositions);

        var lamda = new Func<IQueryable<Domain.Entities.Batch>, IQueryable<Domain.Entities.Batch>>(q =>
        {
            return q.Include(x => x.BatchItems)
                    .Include(x => x.BatchInAerationPositions)
                    .ThenInclude(x => x.AerationPosition);
        });

        var batch = await _batchRepository
                .FindAll(x => x.Id.Equals(request.Id), false, lamda)
                .SingleOrDefaultAsync(cancellationToken);

        if (batch == null)
        {
            return Result<BatchDto>.Failure<BatchDto>(new Error("404", "Batch Not Found"));
        }

        var result = _mapper.Map<BatchDto>(batch);

        result.QrCodeSvg = await _qrCodeServices.GenerateQrCodeAsync(batch.QRCode, cancellationToken);

        return Result<BatchDto>.Success(result);
    }
}
