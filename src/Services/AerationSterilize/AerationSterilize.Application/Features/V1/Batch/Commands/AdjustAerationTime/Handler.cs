using AerationSterilize.Application.Abstractions;
using Contracts.BackgroundJobs;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;

namespace AerationSterilize.Application.Features.V1.Batch.Commands.AdjustAerationTime;
internal class AdjustAerationTimeCommandHandler : ICommandHandler<AdjustAerationTimeCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Batch, int> _batchRepository;
    private readonly IRepositoryBase<Domain.Entities.BatchItem, int> _batchItemRepository;
    private readonly IScheduledJobService _scheduledJobService;

    public AdjustAerationTimeCommandHandler(
        IRepositoryBase<Domain.Entities.Batch, int> batchRepository,
        IRepositoryBase<Domain.Entities.BatchItem, int> batchItemRepository,
        IScheduledJobService scheduledJobService)
    {
        _batchRepository = batchRepository ?? throw new ArgumentNullException(nameof(batchRepository));
        _batchItemRepository = batchItemRepository ?? throw new ArgumentNullException(nameof(batchItemRepository));
        _scheduledJobService = scheduledJobService ?? throw new ArgumentNullException(nameof(scheduledJobService));
    }

    public async Task<Result> Handle(AdjustAerationTimeCommand request, CancellationToken cancellationToken)
    {
        var batches = await _batchRepository
            .FindAll(x => request.BatchId.Contains(x.Id), tracking: true)
            .ToListAsync(cancellationToken);

        if (batches.Count == 0)
            return Result.Failure(new Error("404", "No batches found"));

        foreach (var batch in batches)
        {
            if (batch.PlanOutputAeration is null)
                return Result.Failure(new Error("400", $"Batch {batch.Id} has not been input to aeration room yet"));

            // Delete old scheduled job
            if (!string.IsNullOrEmpty(batch.HangfireJobId))
                _scheduledJobService.Delete(batch.HangfireJobId);

            // Update plan output time
            batch.PlanOutputAeration = batch.PlanOutputAeration.Value.AddHours(request.AerationTime);

            // Update batch items
            var batchItems = await _batchItemRepository
                .FindAll(x => x.BatchId == batch.Id, tracking: true)
                .ToListAsync(cancellationToken);

            foreach (var item in batchItems)
                item.PlanOutputAerationDate = item.PlanOutputAerationDate?.AddHours(request.AerationTime);

            // Schedule new job with updated time
            var newJobId = _scheduledJobService.Schedule<IAerationScheduledJob>(
                job => job.ExecuteOutputAerationAsync(batch.Id), batch.PlanOutputAeration.Value);

            batch.HangfireJobId = newJobId;
        }

        return Result.Success();
    }
}
