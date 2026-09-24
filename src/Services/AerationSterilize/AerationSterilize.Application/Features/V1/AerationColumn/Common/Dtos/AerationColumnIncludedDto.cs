using AerationSterilize.Application.Features.V1.AerationPosition.Common.Dtos;
using AerationSterilize.Domain.Enums;

namespace AerationSterilize.Application.Features.V1.AerationColumn.Common.Dtos;
public class AerationColumnIncludedDto
{
    public int Id { get; set; }
    public string ColumnName { get; set; }
    public bool? Status { get; set; }

    public ICollection<AerationPositionDto> AerationPositionDtos { get; set; }
}
