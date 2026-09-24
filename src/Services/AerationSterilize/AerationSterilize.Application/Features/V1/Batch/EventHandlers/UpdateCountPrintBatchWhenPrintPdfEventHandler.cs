using AerationSterilize.Application.Features.V1.Batch.Events;
using AerationSterilize.Domain.Entities;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Serilog;

namespace AerationSterilize.Application.Features.V1.Batch.EventHandlers;
internal class UpdateCountPrintBatchWhenPrintPdfEventHandler : IDomainEventHandler<UpdateCountPrintBatchWhenPrintPdfEvent>
{
    private readonly IRepositoryBase<Domain.Entities.Batch, int> _batchRepository;
    private readonly IUnitOfWork _unitOfWork;
    private readonly ILogger _logger;

    public UpdateCountPrintBatchWhenPrintPdfEventHandler(IRepositoryBase<Domain.Entities.Batch, int> batchRepository,
        IUnitOfWork unitOfWork,
        ILogger logger)
    {
        _batchRepository = batchRepository ?? throw new ArgumentNullException(nameof(batchRepository));
        _unitOfWork = unitOfWork ?? throw new ArgumentNullException(nameof(unitOfWork));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task Handle(UpdateCountPrintBatchWhenPrintPdfEvent notification, CancellationToken cancellationToken)
    {
        var batchIds = notification.BatchId;

        var batches = await _batchRepository
            .FindAll(x => batchIds.Contains(x.Id), true)
            .ToListAsync(cancellationToken);

        if (batches.Count == 0)
        {
            _logger.Error("Batch Not Found: {batchId}");
            return;
        }
        foreach (var item in batches)
        {
            item.Print += 1;
        }

        await _unitOfWork.SaveChangesAsync(cancellationToken);
    }
}
