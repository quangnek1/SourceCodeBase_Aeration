using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;

namespace AerationSterilize.Application.Features.V1.Setting.Commands.CreateSetting;

internal sealed class CreateSettingCommandHandler : ICommandHandler<CreateSettingCommand>
{
    private readonly IRepositoryBase<Domain.Entities.Setting, int> _repository;
    private readonly IMapper _mapper;

    public CreateSettingCommandHandler(
        IRepositoryBase<Domain.Entities.Setting, int> repository,
        IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result> Handle(CreateSettingCommand request, CancellationToken cancellationToken)
    {
        var setting = await _repository.FindAll().AnyAsync();
        if (setting != null)
        {
            return Result.Failure(new Error("400", "Setting already exists. Only one setting is allowed."));
        }
        var entity = _mapper.Map<Domain.Entities.Setting>(request);

        _repository.Add(entity);

        return Result.Success("Setting created successfully.");
    }
}
