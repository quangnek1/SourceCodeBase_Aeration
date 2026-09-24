using System.Collections.Generic;
using AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
using AerationSterilize.Domain.Entities;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Contracts.Services.QRCode;
using Microsoft.EntityFrameworkCore;

namespace AerationSterilize.Application.Features.V1.Batch.Queries.GetBatchsForPrint;
internal class GetBatchsForPrintQueryHandler : IQueryHandler<GetBatchsForPrintQuery, List<BatchDto>>
{
    private readonly IRepositoryBase<Domain.Entities.Batch, int> _batchRepository;
    private readonly IMapper _mapper;
    private readonly IQrCodeServices _qrCodeServices;

    public GetBatchsForPrintQueryHandler(IRepositoryBase<Domain.Entities.Batch, int> batchRepository, IMapper mapper,
        IQrCodeServices qrCodeServices)
    {
        _batchRepository = batchRepository ?? throw new ArgumentNullException(nameof(batchRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _qrCodeServices = qrCodeServices ?? throw new ArgumentNullException(nameof(qrCodeServices));
    }
 
    public async Task<Result<List<BatchDto>>> Handle(GetBatchsForPrintQuery request, CancellationToken cancellationToken)
    {
        var lamda = new Func<IQueryable<Domain.Entities.Batch>, IQueryable<Domain.Entities.Batch>>(q =>
        {
            return q.Include(x => x.BatchItems)
                    .Include(x => x.BatchInAerationPositions)
                    .ThenInclude(x => x.AerationPosition);
        });

        var batches = await _batchRepository
                .FindAll(x => request.BatchIds.Contains(x.Id), false, lamda)
                .ToListAsync(cancellationToken);

        var result = _mapper.Map<List<BatchDto>>(batches);

        foreach (var item in result)
        {
            item.QrCodeSvg = await _qrCodeServices.GenerateQrCodeAsync(item.QRCode, cancellationToken);
        }

        return Result<IReadOnlyList<BatchDto>>.Success(result);
    }
}
