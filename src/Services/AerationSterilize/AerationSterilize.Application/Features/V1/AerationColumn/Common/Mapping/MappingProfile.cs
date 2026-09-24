using AerationSterilize.Application.Features.V1.AerationColumn.Commands.CreateAerationColumn;
using AerationSterilize.Application.Features.V1.AerationColumn.Commands.UpdateAerationColumn;
using AerationSterilize.Application.Features.V1.AerationColumn.Common.Dtos;
using AerationSterilize.Application.Features.V1.AerationPosition.Common.Dtos;
using AerationSterilize.Domain.Entities;
using AerationSterilize.Domain.Enums;
using AutoMapper;
using Shared.Emumerations;
using Shared.Paging;

namespace AerationSterilize.Application.Features.V1.AerationColumn.Common.Mapping;

public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UpdateAerationColumnCommand, Domain.Entities.AerationColumn>();
        CreateMap<Domain.Entities.AerationColumn, AerationColumnDto>();
        CreateMap<PagedResult<Domain.Entities.AerationColumn>, PagedResult<AerationColumnDto>>().ReverseMap();

        CreateMap<Domain.Entities.AerationColumn, AerationColumnIncludedDto>()
            .ForMember(dest => dest.AerationPositionDtos, opt => opt.MapFrom(src => src.AerationPositions))
            ;

        CreateMap<Domain.Entities.AerationPosition, AerationPositionDto>()
            .ForMember(dest => dest.BatchInAerationPositionDataDtos,
                opt => opt.MapFrom(src => src.BatchInAerationPositions))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src =>
                    src.BatchInAerationPositions.Any(x => x.Batch.Status == DataStatus.Aeration)
                        ? AerationStatus.InProgress
                        : src.BatchInAerationPositions.Any(x => x.Batch.Status == DataStatus.AerationDone)
                            ? AerationStatus.Done
                            : AerationStatus.Empty));

        CreateMap<Domain.Entities.BatchInAerationPosition, BatchInAerationPositionDataDto>()
            .ForMember(dest => dest.BatchDto,
                opt => opt.MapFrom(src => src.Batch));

     
    }
}
