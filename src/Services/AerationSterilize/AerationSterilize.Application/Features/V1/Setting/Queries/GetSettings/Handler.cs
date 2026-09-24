using AerationSterilize.Application.Features.V1.Setting.Common.Dtos;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Microsoft.EntityFrameworkCore;

namespace AerationSterilize.Application.Features.V1.Setting.Queries.GetSettings;

public class GetSettingsQueryHandler : IQueryHandler<GetSettingQuery, SettingDto>
{
    private readonly IRepositoryBase<Domain.Entities.Setting, int> _repository;
    private readonly IMapper _mapper;

    public GetSettingsQueryHandler(
        IRepositoryBase<Domain.Entities.Setting, int> repository,
        IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<SettingDto>> Handle(GetSettingQuery request, CancellationToken cancellationToken)
    {
        var settingsQuery = await _repository.FindAll(null, false).FirstOrDefaultAsync();

        var result = _mapper.Map<SettingDto>(settingsQuery);

        return Result.Success(result);
    }
}
