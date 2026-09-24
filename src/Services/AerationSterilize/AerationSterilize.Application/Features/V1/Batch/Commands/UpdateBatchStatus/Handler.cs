using AerationSterilize.Domain.Enums;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;
using Shared.Emumerations;

namespace AerationSterilize.Application.Features.V1.Batch.Commands.UpdateBatchStatus;
internal class UpdateBatchStatusCommandHandler : ICommandHandler<UpdateBatchStatusCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Batch, int> _batchRepository;
    private readonly IRepositoryBase<Domain.Entities.AerationPosition, int> _aerationRepository;
    private readonly IRepositoryBase<Domain.Entities.BatchInAerationPosition, int> _batchInRepository;

    public UpdateBatchStatusCommandHandler(IRepositoryBase<Domain.Entities.Batch, int> batchRepository,
        IRepositoryBase<Domain.Entities.AerationPosition, int> aerationRepository,
        IRepositoryBase<Domain.Entities.BatchInAerationPosition, int> batchInRepository)
    {
        _aerationRepository = aerationRepository ?? throw new ArgumentNullException(nameof(aerationRepository));
        _batchRepository = batchRepository ?? throw new ArgumentNullException(nameof(batchRepository));
        _batchInRepository = batchInRepository ?? throw new ArgumentNullException(nameof(batchInRepository));
    }

    public async Task<Result> Handle(UpdateBatchStatusCommand request, CancellationToken cancellationToken)
    {
        var batch = await _batchRepository.FindByIdAsync(request.BatchId, cancellationToken);

        if (batch == null)
        {
            return Result.Failure(new Error("404", "Batch Not Found"));
        }

        batch.Status = DataStatus.AerationDone;

        var positions = await _aerationRepository.FindAll(
                       x => x.BatchInAerationPositions.Any(b => b.BatchId == request.BatchId), true)
                    .ToListAsync(cancellationToken);

        foreach (var item in positions)
        {
            item.Status = AerationStatus.Done;
        }

        return Result.Success("Update Successfuly");
    }
}
