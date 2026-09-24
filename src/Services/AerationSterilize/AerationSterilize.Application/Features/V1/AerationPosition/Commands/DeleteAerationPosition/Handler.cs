using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;

namespace AerationSterilize.Application.Features.V1.AerationPosition.Commands.DeleteAerationPosition;

internal sealed class DeleteAerationPositionCommandHandler : ICommandHandler<DeleteAerationPositionCommand>
{
    private readonly IRepositoryBase<Domain.Entities.AerationPosition, int> _repository;

    public DeleteAerationPositionCommandHandler(
        IRepositoryBase<Domain.Entities.AerationPosition, int> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result> Handle(DeleteAerationPositionCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.FindByIdAsync(request.Id);

        _repository.Remove(entity);

        return Result.Success();
    }
}
