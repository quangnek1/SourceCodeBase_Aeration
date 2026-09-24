using AerationSterilize.Application.Features.V1.BoxingJob.Common.Dtos;
using AutoMapper;
using BoxingJobs = AerationSterilize.Domain.Entities.Working.BoxingJob;

namespace AerationSterilize.Application.Features.V1.BoxingJob.Common.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BoxingJobs, BoxingJobDto>().ReverseMap();
    }

}
