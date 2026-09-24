namespace AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
public class BatchPrintPdfDto
{
    public int Id { get; set; }
    public DateTimeOffset SterilizeDate { get; set; }
    public DateTimeOffset CreatedDate { get; set; }
    public string BatchNo { get; set; }
    public int Keep { get; set; }
    public int Print { get; set; }
    public string? Status { get; set; }

    //public ICollection<BatchInAerationPositionDto> BatchInAerationPositionDtos { get; set; }
    //public ICollection<BatchItemDto> BatchItemDtos { get; set; }
}
