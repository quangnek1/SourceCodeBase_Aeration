using AerationSterilize.Application.Features.V1.AerationPosition.Common.Dtos;
using AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
using AerationSterilize.Application.Features.V1.BoxingJob.Common.Dtos;
using AutoMapper;
using Shared.Paging;

namespace AerationSterilize.Application.Features.V1.Batch.Common.Mapping;
public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<Domain.Entities.Batch, BatchDto>()
            .PreserveReferences()
            .ForMember(dest => dest.BatchItemDtos, opt => opt.MapFrom(src => src.BatchItems))
            .ForMember(dest => dest.BatchInAerationPositionDtos, opt => opt.MapFrom(src => src.BatchInAerationPositions));

        CreateMap<PagedResult<Domain.Entities.Batch>, PagedResult<BatchDto>>().ReverseMap();

        CreateMap<Domain.Entities.BatchItem, BatchItemDto>();

        CreateMap<Domain.Entities.BatchInAerationPosition, BatchInAerationPositionDto>()
            .PreserveReferences()
            .ForMember(dest => dest.BatchDto, opt => opt.MapFrom(src => src.Batch));

        CreateMap<Domain.Entities.AerationPosition, AerationPositionDto>();

        CreateMap<Domain.Entities.Batch, BatchScanQrcodeDto>()
            .ForMember(dest => dest.Status, opt => opt.MapFrom(src => src.Status))
            .ForMember(dest => dest.BatchItemDtos, opt => opt.MapFrom(src => src.BatchItems))
            .ForMember(destinationMember => destinationMember.BatchInAerationPositionDtos,
                opt => opt.MapFrom(src => src.BatchInAerationPositions))
            ;

        CreateMap<Domain.Entities.PackingPosition, PackingPositionDto>();
        CreateMap<Domain.Entities.Working.Box, BoxDto>()
            .ForMember(d => d.ItemTag, o => o.MapFrom(s => s.ItemTags));
        CreateMap<Domain.Entities.Working.ItemTag, ItemTagDto>();
        CreateMap<Domain.Entities.BatchItem, ItemProgessDto>()
            .ForMember(dest => dest.BatchDto, opt => opt.MapFrom(src => src.Batch))
            .ForMember(d => d.CurrentQty, o => o.MapFrom(s => s.Boxes == null
            ? 0
            : s.Boxes.SelectMany(b => b.ItemTags).Sum(p => p.Qty)))
            .ForMember(d => d.TargetQty, o => o.MapFrom(s => s.QtyInput));
        ;

        CreateMap<PagedResult<Domain.Entities.BatchItem>, PagedResult<ItemProgessDto>>()
            .ReverseMap();

        CreateMap<Domain.Entities.Batch, BatchPrintPdfDto>();
        CreateMap<PagedResult<Domain.Entities.Batch>, PagedResult<BatchPrintPdfDto>>();

    }
}
