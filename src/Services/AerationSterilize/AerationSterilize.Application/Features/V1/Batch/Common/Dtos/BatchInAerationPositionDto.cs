using AerationSterilize.Application.Features.V1.AerationPosition.Common.Dtos;

namespace AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
public class BatchInAerationPositionDto
{
    public AerationPositionDto AerationPosition { get; set; }
    public BatchDto BatchDto { get; set; }
}
