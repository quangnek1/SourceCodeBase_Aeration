using AerationSterilize.Application.Features.V1.BoxingJob.Common.Dtos;

namespace AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
public class ItemProgessDto
{
    public int Id { get; set; }
    public BatchDto BatchDto { get; set; }
    public string? InternalLot { get; set; }
    public int KeepAeration { get; set; }
    public string? Status { get; set; }

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

    public BatchItemInPackingPositionDto BatchItemInPackingPosition { get; set; }

    public string? BoxingPosition { get; set; }
    public int? TargetQty { get; set; }
    public int? CurrentQty { get; set; }

    public ICollection<BoxDto> Boxes { get; set; }

}

