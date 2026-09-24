namespace AerationSterilize.Application.Features.V1.BoxingJob.Common.Dtos;
public class ScanPositionDto
{
    public int PositionId { get; set; }

    public string PositionCode { get; set; }

    public bool HasActiveJob { get; set; }

    public int? JobId { get; set; }

    public string? JobNo { get; set; }

    public int TargetQty { get; set; }

    public int? ActualQty { get; set; }
}
