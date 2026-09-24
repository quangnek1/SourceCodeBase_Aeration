using AerationSterilize.Domain.Entities.Working;
using Contracts.Abstractions.Entities.Domains;
using Shared.Emumerations;

namespace AerationSterilize.Domain.Entities;
public class BatchItem : EntityAuditBase<int>
{
    public int BatchId { get; set; }
    public int DataPlanId { get; set; }
    public int? DataAmiQ411Id { get; set; }

    public Batch Batch { get; set; }
    public DataPlan DataPlan { get; set; }
    public DataAmiQ411 DataAmiQ411 { get; set; }

    // Snapshot
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
    public DataStatus? Status { get; set; }

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

    // Navigation Properties
    public int? PackingPositionId { get; set; }
    public PackingPosition? PackingPosition { get; set; }
    public ICollection<Box> Boxes { get; set; }

}
