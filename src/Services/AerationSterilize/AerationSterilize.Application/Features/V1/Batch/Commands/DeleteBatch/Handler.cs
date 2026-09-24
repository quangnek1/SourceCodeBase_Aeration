using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using Shared.Emumerations;

namespace AerationSterilize.Application.Features.V1.Batch.Commands.DeleteBatch;
internal class DeleteBatchCommandHandler : ICommandHandler<DeleteBatchCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Batch, int> _batchRepository;
    private readonly IRepositoryBase<Domain.Entities.BatchItem, int> _batchItemRepository;
    private readonly IRepositoryBase<Domain.Entities.BatchInAerationPosition, int> _batchInAerationPositionRepository;

    public DeleteBatchCommandHandler(
        IRepositoryBase<Domain.Entities.Batch, int> batchRepository,
        IRepositoryBase<Domain.Entities.BatchItem, int> batchItemRepository,
        IRepositoryBase<Domain.Entities.BatchInAerationPosition, int> batchInAerationPositionRepository)
    {
        _batchRepository = batchRepository ?? throw new ArgumentNullException(nameof(batchRepository));
        _batchItemRepository = batchItemRepository ?? throw new ArgumentNullException(nameof(batchItemRepository));
        _batchInAerationPositionRepository = batchInAerationPositionRepository ?? throw new ArgumentNullException(nameof(batchInAerationPositionRepository));
    }

    public async Task<Result> Handle(DeleteBatchCommand request, CancellationToken cancellationToken)
    {
        var batch = await _batchRepository.FindByIdAsync(request.BatchId, cancellationToken);

        if (batch is null)
            return Result.Failure(new Error("404", $"Không tìm thấy mẻ với Id = {request.BatchId}."));

        // Kiểm tra mẻ đã được InputAeration chưa
        if (batch.Status != DataStatus.Aeration)
            return Result.Failure(new Error("400", "Không thể xóa mẻ đã được đưa vào phòng aeration."));

        var batchItems = await _batchItemRepository
            .FindAll(x => x.BatchId == request.BatchId, tracking: true)
            .ToListAsync(cancellationToken);

        _batchItemRepository.RemoveMultiple(batchItems);

        var batchPositions = await _batchInAerationPositionRepository
            .FindAll(x => x.BatchId == request.BatchId, tracking: true)
            .ToListAsync(cancellationToken);

        _batchInAerationPositionRepository.RemoveMultiple(batchPositions);

        _batchRepository.Remove(batch);

        return Result.Success();
    }
}
