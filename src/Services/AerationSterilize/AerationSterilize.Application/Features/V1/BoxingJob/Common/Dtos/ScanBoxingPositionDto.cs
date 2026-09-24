namespace AerationSterilize.Application.Features.V1.BoxingJob.Common.Dtos;
public class ScanBoxingPositionDto
{
    public int PositionId { get; set; }
    public string PositionCode { get; set; }
    public bool HasActiveJob { get; set; }
    public ICollection<BoxingJobDto>? BoxingJobDtos { get; set; } = [];

}
