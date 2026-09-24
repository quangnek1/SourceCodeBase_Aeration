using AerationSterilize.Application.Abstractions;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Shared.Emumerations;

namespace AerationSterilize.Application.Features.V1.Batch.Commands.OutputAerationRoom;

internal class OutputAerationRoomCommandHandler : ICommandHandler<OutputAerationRoomCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Batch, int> _batchRepository;
    private readonly IRepositoryBase<Domain.Entities.BatchItem, int> _batchItemRepository;
    private readonly IRepositoryBase<Domain.Entities.AerationPosition, int> _aerationPositionRepository;
    private readonly IRepositoryBase<Domain.Entities.BatchInAerationPosition, int> _batchInAerationPositionRepository;

    public OutputAerationRoomCommandHandler(
      IRepositoryBase<Domain.Entities.BatchInAerationPosition, int> batchInAerationPositionRepository,
      IRepositoryBase<Domain.Entities.Batch, int> batchRepository,
      IRepositoryBase<Domain.Entities.BatchItem, int> batchItemRepository,
      IRepositoryBase<Domain.Entities.AerationPosition, int> aerationPositionRepository
      )
    {
        _batchInAerationPositionRepository = batchInAerationPositionRepository ?? throw new ArgumentNullException(nameof(batchInAerationPositionRepository));
        _batchRepository = batchRepository ?? throw new ArgumentNullException(nameof(batchRepository));
        _batchItemRepository = batchItemRepository ?? throw new ArgumentNullException(nameof(batchItemRepository));
        _aerationPositionRepository = aerationPositionRepository ?? throw new ArgumentNullException(nameof(aerationPositionRepository));
    }

    public async Task<Result> Handle(OutputAerationRoomCommand request, CancellationToken cancellationToken)
    {
        var batch = await _batchRepository.FindByIdAsync(request.BatchId, cancellationToken,
            p => p.BatchItems, p => p.BatchInAerationPositions);

        if (batch == null)
        {
            return Result.Failure(new Error("404", "Batch Not Found"));
        }

        var batchItemsOut = batch.BatchItems
                .Where(x => request.BatchItems.Contains(x.Id) && x.ActualOutputAerationDate == null)
                .ToList();

        if (batchItemsOut.Count == 0)
        {
            return Result.Failure(new Error("400", "Không còn mẻ nào chưa Out."));
        }

        var dataStatus = DataStatus.Boxing;

        foreach (var item in batchItemsOut)
        {
            var date = request.Date;

            item.ActualOutputAerationDate = date;
            item.InputBoxingDate = date;
            item.Status = dataStatus;
        }

        // Kiểm tra nếu tất cả BatchItem đã được output khỏi Aeration Room,
        // nếu có thì update Batch status và Aeration status,
        // đồng thời update status của các Position liên quan về Empty
        if (batch.BatchItems.Where(x => x.ActualOutputAerationDate == null).Count() == 0)
        {
            batch.Status = dataStatus;
            batch.ActualOutputAeration = batch.BatchItems.Max(x => x.ActualOutputAerationDate);

            var positions = _aerationPositionRepository
                .FindAll(x => batch.BatchInAerationPositions
                .Select(p => p.AerationPositionId).Contains(x.Id), tracking: true)
                .ToList();

            foreach (var item in positions)
            {
                item.Status = Domain.Enums.AerationStatus.Empty;
            }
        }

        return Result.Success();
    }
}
