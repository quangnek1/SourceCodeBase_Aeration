namespace AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
public class BatchDto
{
    public int Id { get; set; }
    public string QRCode { get; set; }
    public string QrCodeSvg { get; set; }
    public DateTimeOffset SterilizeDate { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string BatchNo { get; set; }
    public int Keep { get; set; }
    public string? Status { get; set; }
    // Aeration
    public DateTimeOffset? InputAerationActual { get; set; }
    public DateTimeOffset? PlanOutputAeration { get; set; }
    public DateTimeOffset? ActualOutputAeration { get; set; }

    public ICollection<BatchInAerationPositionDto> BatchInAerationPositionDtos { get; set; }

    public ICollection<BatchItemDto> BatchItemDtos { get; set; }

}
