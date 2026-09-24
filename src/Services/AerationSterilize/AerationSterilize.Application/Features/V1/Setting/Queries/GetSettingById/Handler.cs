using AerationSterilize.Application.Features.V1.Setting.Common.Dtos;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;

namespace AerationSterilize.Application.Features.V1.Setting.Queries.GetSettingById;

public class GetSettingByIdQueryHandler : IQueryHandler<GetSettingByIdQuery, SettingDto>
{
    private readonly IRepositoryBase<Domain.Entities.Setting, int> _repository;
    private readonly IMapper _mapper;

    public GetSettingByIdQueryHandler(
        IRepositoryBase<Domain.Entities.Setting, int> repository,
        IMapper mapper)
    {
        _repository = repository ?? throw new ArgumentNullException(nameof(repository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
    }

    public async Task<Result<SettingDto>> Handle(GetSettingByIdQuery request, CancellationToken cancellationToken)
    {
        var entity = await _repository.FindByIdAsync(request.Id);

        var result = _mapper.Map<SettingDto>(entity);

        return Result.Success(result);
    }
}
