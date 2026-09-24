using AerationSterilize.Application.Features.V1.DataPlan.Common.Dtos;
using AutoMapper;
using Shared.Emumerations;

namespace AerationSterilize.Application.Features.V1.DataPlan.Common.Mapping;
public sealed class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<DataPlanDto, Domain.Entities.DataPlan>();

        CreateMap<Domain.Entities.DataPlan, DataPlanDto>()
           .ForMember(dest => dest.StatusDescription, opt => opt.MapFrom(src => src.Status.HasValue ? src.Status.Value.ToString() : null));
        //CreateMap<IReadOnlyList<DataPlanDto>, IReadOnlyList<Domain.Entities.DataPlan>>();
    }
}
