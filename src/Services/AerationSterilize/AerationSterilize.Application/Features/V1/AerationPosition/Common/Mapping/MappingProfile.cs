using AerationSterilize.Application.Features.V1.AerationColumn.Common.Dtos;
using AerationSterilize.Application.Features.V1.AerationPosition.Commands.CreateAerationPosition;
using AerationSterilize.Application.Features.V1.AerationPosition.Commands.UpdateAerationPosition;
using AerationSterilize.Application.Features.V1.AerationPosition.Common.Dtos;
using AerationSterilize.Domain.Enums;
using AutoMapper;
using Shared.Emumerations;
using Shared.Paging;

namespace AerationSterilize.Application.Features.V1.AerationPosition.Common.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<CreateAerationPositionCommand, Domain.Entities.AerationPosition>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.HasValue ? AerationStatus.FromValue(src.Status.Value) : null));

        CreateMap<UpdateAerationPositionCommand, Domain.Entities.AerationPosition>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status.HasValue ? AerationStatus.FromValue(src.Status.Value) : null));

        CreateMap<Domain.Entities.AerationPosition, AerationPositionDto>();

        CreateMap<PagedResult<Domain.Entities.AerationPosition>, PagedResult<AerationPositionDto>>().ReverseMap();

        CreateMap<Domain.Entities.AerationColumn, AerationColumnIncludedDto>();


    }
}
