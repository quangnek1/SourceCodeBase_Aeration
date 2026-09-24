using Contracts.Abstractions.Entities.Domains;

namespace AerationSterilize.Domain.Entities;
public class DataAmiQ411 : EntityAuditBase<int>
{
    public string ProductName { get; set; } = null!;
    public string ProductInformation { get; set; } = null!;
    public string DrawingNumber { get; set; } = null!;
    public string CatalogCode { get; set; } = null!;
    public string Destination { get; set; } = null!;
    public int? ChamberA { get; set; }
    public int? ChamberB { get; set; }
    public int? ChamberC { get; set; }
    public int? ChamberD { get; set; }
    public bool? Status { get; set; }
}
