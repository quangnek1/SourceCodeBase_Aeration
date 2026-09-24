using AerationSterilize.Application.Features.V1.Setting.Common.Dtos;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Exceptions;
using Contracts.Responses;

namespace AerationSterilize.Application.Features.V1.Setting.Commands.UpdateSetting;

internal sealed class UpdateSettingCommandHandler : ICommandHandler<UpdateSettingCommand, SettingDto>
{
    private readonly IRepositoryBase<Domain.Entities.Setting, int> _repository;
    private readonly IMapper _mapper;

    public UpdateSettingCommandHandler(
        IRepositoryBase<Domain.Entities.Setting, int> repository,
        IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<SettingDto>> Handle(UpdateSettingCommand request, CancellationToken cancellationToken)
    {
        var validationErrors = new List<ValidationError>();

        ValidateFilePath(request.PlanCAG, nameof(request.PlanCAG), validationErrors);
        ValidateFilePath(request.PlanPTCA, nameof(request.PlanPTCA), validationErrors);
        ValidateFilePath(request.DataAmiQ411, nameof(request.DataAmiQ411), validationErrors);

        if (validationErrors.Count > 0)
        {
            throw new ValidationException(validationErrors);
        }

        var entity = await _repository.FindByIdAsync(request.Id);

        entity = _mapper.Map(request, entity);

        _repository.Update(entity);

        var result = _mapper.Map<SettingDto>(entity);

        return Result.Success(result);
    }

    private static void ValidateFilePath(string? filePath, string propertyName, ICollection<ValidationError> validationErrors)
    {
        if (string.IsNullOrWhiteSpace(filePath) || !File.Exists(filePath))
        {
            validationErrors.Add(new ValidationError(propertyName, "The specified file does not exist."));
        }
    }
}
