using AerationSterilize.Application.Features.V1.AerationColumn.Common.Dtos;
using AerationSterilize.Domain.Enums;

namespace AerationSterilize.Application.Features.V1.AerationPosition.Common.Dtos;
public class AerationPositionDto
{
    public int Id { get; set; }
    public string PositionCode { get; set; }
    public string? Image { get; set; }
    public int AerationColumnId { get; set; }
    public AerationStatus? Status { get; set; }
    public ICollection<BatchInAerationPositionDataDto> BatchInAerationPositionDataDtos { get; set; }

}
