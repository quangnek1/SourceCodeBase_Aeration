using AerationSterilize.Domain.Entities;
using AerationSterilize.Domain.Enums;
using Contracts.BackgroundJobs;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using Shared.Emumerations;
using AerationSterilize.Application.Abstractions;

namespace AerationSterilize.Application.Features.V1.Batch.Commands.InputAerationRoom;
internal class InputAerationRoomCommandHandler : ICommandHandler<InputAerationRoomCommand>
{
    private readonly IRepositoryBase<Domain.Entities.BatchInAerationPosition, int> _batchInAerationPositionRepository;
    private readonly IRepositoryBase<Domain.Entities.Batch, int> _batchRepository;
    private readonly IRepositoryBase<Domain.Entities.BatchItem, int> _batchItemRepository;
    private readonly IRepositoryBase<Domain.Entities.AerationPosition, int> _aerationPositionRepository;
    private readonly IScheduledJobService _scheduledJobService;

    public InputAerationRoomCommandHandler(
        IRepositoryBase<Domain.Entities.BatchInAerationPosition, int> batchInAerationPositionRepository,
        IRepositoryBase<Domain.Entities.Batch, int> batchRepository,
        IRepositoryBase<Domain.Entities.BatchItem, int> batchItemRepository,
        IRepositoryBase<Domain.Entities.AerationPosition, int> aerationPositionRepository,
        IScheduledJobService scheduledJobService
        )
    {
        _batchInAerationPositionRepository = batchInAerationPositionRepository ?? throw new ArgumentNullException(nameof(batchInAerationPositionRepository));
        _batchRepository = batchRepository ?? throw new ArgumentNullException(nameof(batchRepository));
        _batchItemRepository = batchItemRepository ?? throw new ArgumentNullException(nameof(batchItemRepository));
        _aerationPositionRepository = aerationPositionRepository ?? throw new ArgumentNullException(nameof(aerationPositionRepository));
        _scheduledJobService = scheduledJobService ?? throw new ArgumentNullException(nameof(scheduledJobService));
    }

    public async Task<Result> Handle(InputAerationRoomCommand request, CancellationToken cancellationToken)
    {
        // Update Batch status to InProgress and Aeration
        var batch = await _batchRepository.FindByIdAsync(request.batchId, cancellationToken);
        if (batch == null)
        {
            return Result.Failure(new Error("404", "Batch Not Found"));
        }

        var dataStatus = DataStatus.Aeration;

        batch.Status = dataStatus;
        batch.InputAerationActual = request.Date;
        batch.PlanOutputAeration = request.Date.AddDays(batch.Keep);

        // Update BatchItem DateTime to now and status to InProgress and Aeration
        var batchItems = await _batchItemRepository
            .FindAll(x => x.BatchId == request.batchId, true)
            .ToListAsync(cancellationToken);

        foreach (var item in batchItems)
        {
            item.InputAerationDate = request.Date;
            item.PlanOutputAerationDate = request.Date.AddDays(batch.Keep);
            item.Status = dataStatus;
        }

        //Update Position
        var positions = await _aerationPositionRepository
            .FindAll(x => request.positionIds.Contains(x.Id), tracking: true)
            .ToListAsync(cancellationToken);

        var batchPositions = new List<BatchInAerationPosition>();

        foreach (var position in positions)
        {
            position.Status = AerationStatus.InProgress;

            batchPositions.Add(new BatchInAerationPosition
            {
                BatchId = request.batchId,
                AerationPositionId = position.Id
            });
        }

        _batchInAerationPositionRepository.AddList(batchPositions);

        // Schedule Hangfire job to auto-output at PlanOutputAeration time
        var jobId = _scheduledJobService.Schedule<IAerationScheduledJob>(
            job => job.ExecuteOutputAerationAsync(request.batchId), batch.PlanOutputAeration!.Value);

        batch.HangfireJobId = jobId;

        return Result.Success();
    }
}
