using AerationSterilize.Domain.Enums;

namespace AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
public class PackingPositionDto
{
    public string PositionCode { get; set; }
    public string? Image { get; set; }
    public AerationStatus? Status { get; set; }
}
