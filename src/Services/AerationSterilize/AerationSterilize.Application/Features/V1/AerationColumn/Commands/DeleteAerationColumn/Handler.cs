using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;

namespace AerationSterilize.Application.Features.V1.AerationColumn.Commands.DeleteAerationColumn;

internal sealed class DeleteAerationColumnCommandHandler : ICommandHandler<DeleteAerationColumnCommand>
{
    private readonly IRepositoryBase<Domain.Entities.AerationColumn, int> _repository;

    public DeleteAerationColumnCommandHandler(
        IRepositoryBase<Domain.Entities.AerationColumn, int> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result> Handle(DeleteAerationColumnCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.FindByIdAsync(request.Id);

        _repository.Remove(entity);

        return Result.Success();
    }
}
