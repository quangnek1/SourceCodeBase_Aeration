using AerationSterilize.Application.Abstractions;
using AerationSterilize.Application.Features.V1.Batch.Commands.UpdateBatchStatus;
using Contracts.Common.Repositories;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AerationSterilize.Infrastructure.Services;

public class AerationScheduledJobService : IAerationScheduledJob
{
    private readonly ISender _sender;
    private readonly IRepositoryBase<Domain.Entities.BatchItem, int> _batchItemRepository;
    private readonly ILogger<AerationScheduledJobService> _logger;
    private readonly ISignalRServices _signalRServices;

    public AerationScheduledJobService(
        ISender sender,
        IRepositoryBase<Domain.Entities.BatchItem, int> batchItemRepository,
        ILogger<AerationScheduledJobService> logger,
        ISignalRServices signalRServices)
    {
        _sender = sender ?? throw new ArgumentNullException(nameof(sender));
        _batchItemRepository = batchItemRepository ?? throw new ArgumentNullException(nameof(batchItemRepository));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        _signalRServices = signalRServices ?? throw new ArgumentNullException(nameof(signalRServices));
    }

    public async Task ExecuteOutputAerationAsync(int batchId)
    {
        _logger.LogInformation("Hangfire: Executing scheduled OutputAeration job for BatchId: {BatchId}", batchId);

        var batchItemIds = await _batchItemRepository
            .FindAll(x => x.BatchId == batchId && x.ActualOutputAerationDate == null)
            .Select(x => x.Id)
            .ToListAsync();

        if (batchItemIds.Count == 0)
        {
            _logger.LogWarning("Hangfire: No pending batch items found for BatchId: {BatchId}. Job skipped.", batchId);
            return;
        }

        var command = new UpdateBatchStatusCommand(batchId);
        var result = await _sender.Send(command);

        if (result.IsFailure)
        {
            _logger.LogError("Hangfire: OutputAeration job failed for BatchId: {BatchId}. Error: {Error}", batchId, result.Error);
            return;
        }

        _logger.LogInformation("Hangfire: OutputAeration job completed successfully for BatchId: {BatchId}", batchId);
    }
}
