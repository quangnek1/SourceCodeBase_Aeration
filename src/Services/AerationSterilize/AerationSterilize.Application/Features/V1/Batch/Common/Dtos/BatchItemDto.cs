namespace AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
public class BatchItemDto
{
    public int Id { get; set; }
    public int BatchId { get; set; }
    public int DataPlanId { get; set; }
    public string? NumberRank { get; set; }
    public string? Material { get; set; }
    public string? DrawingListNo { get; set; }
    public string? ItemName { get; set; }
    public string? Destination { get; set; }
    public string? InternalLot { get; set; }
    public string? ExternalLot1 { get; set; }
    public int QtyInput { get; set; }
    public string? A { get; set; }
    public string? B { get; set; }
    public string? C { get; set; }
    public string? D { get; set; }
    public string? Status { get; set; }
    public int Biobudent { get; set; }
    public int Endotoxin { get; set; }
    public int Particle { get; set; }
    public int QtyOutputSealing { get; set; }
    public string? GaugeNoSealing { get; set; }
    public int QtyOutputSterilize { get; set; }
    public int QtyFeaturesTest { get; set; }
    public int KeepAeration { get; set; }
    public string? KeepSample { get; set; }
    public DateTime? DeliveryDatePlan { get; set; }
    public string? Group { get; set; }
    public string Family { get; set; }

    // Aeration
    public DateTimeOffset? InputAerationDate { get; set; }
    public DateTimeOffset? PlanOutputAerationDate { get; set; }
    public DateTimeOffset? ActualOutputAerationDate { get; set; }

    // Boxing
    public DateTimeOffset? InputBoxingDate { get; set; }
    public DateTimeOffset? OutputBoxingDate { get; set; }

    // Packing
    public DateTimeOffset? InputPackingDate { get; set; }
    public DateTimeOffset? OutputPackingDate { get; set; }
}
