using AerationSterilize.Application.Features.V1.Setting.Commands.CreateSetting;
using AerationSterilize.Application.Features.V1.Setting.Commands.UpdateSetting;
using AerationSterilize.Application.Features.V1.Setting.Common.Dtos;
using AutoMapper;
using Shared.Paging;

namespace AerationSterilize.Application.Features.V1.Setting.Common.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateSettingCommand, Domain.Entities.Setting>();
        CreateMap<UpdateSettingCommand, Domain.Entities.Setting>();
        CreateMap<Domain.Entities.Setting, SettingDto>().ReverseMap();
        CreateMap<PagedResult<Domain.Entities.Setting>, PagedResult<SettingDto>>().ReverseMap();
    }
}
