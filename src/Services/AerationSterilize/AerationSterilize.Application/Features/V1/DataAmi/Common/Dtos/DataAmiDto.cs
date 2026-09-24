namespace AerationSterilize.Application.Features.V1.DataAmi.Common.Dtos;
public class DataAmiDto
{
    public int Id { get; set; }
    public string ProductName { get; set; } = string.Empty;
    public string ProductInformation { get; set; } = string.Empty;
    public string DrawingNumber { get; set; } = string.Empty;
    public string CatalogCode { get; set; } = string.Empty;
    public string Destination { get; set; } = string.Empty;
    public int? ChamberA { get; set; }
    public int? ChamberB { get; set; }
    public int? ChamberC { get; set; }
    public int? ChamberD { get; set; }
    public bool? Status { get; set; }
}
