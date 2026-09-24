namespace AerationSterilize.Application.Features.V1.BoxingJob.Common.Dtos;
public class ScanBoxDto
{
    public int Id { get; set; }
    public string BoxCode { get; set; }
    public int CurrentQty { get; set; }
    public int Capacity { get; set; }
}
