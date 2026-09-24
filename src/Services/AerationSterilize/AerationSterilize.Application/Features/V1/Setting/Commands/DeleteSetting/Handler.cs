using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;

namespace AerationSterilize.Application.Features.V1.Setting.Commands.DeleteSetting;

internal sealed class DeleteSettingCommandHandler : ICommandHandler<DeleteSettingCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Setting, int> _repository;

    public DeleteSettingCommandHandler(
        IRepositoryBase<Domain.Entities.Setting, int> repository)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
    }

    public async Task<Result> Handle(DeleteSettingCommand request, CancellationToken cancellationToken)
    {
        var entity = await _repository.FindByIdAsync(request.Id);

        _repository.Remove(entity);

        return Result.Success();
    }
}
