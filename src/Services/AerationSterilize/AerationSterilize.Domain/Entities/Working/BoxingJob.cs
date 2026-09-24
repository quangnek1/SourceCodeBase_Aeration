using AerationSterilize.Domain.Entities.BoxingPositions;
using Contracts.Abstractions.Entities.Domains;
using Shared.Emumerations;

namespace AerationSterilize.Domain.Entities.Working;
public class BoxingJob : EntityAuditBase<int>
{
    public int BoxingPositionId { get; set; }
    public BoxingPosition BoxingPosition { get; set; }

    public int TargetQty { get; set; }
    public BoxingJobStatus Status { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }

    // 1 Job = 1 BatchItem = 1 Lot
    public int BatchItemId { get; set; }
    public BatchItem BatchItem { get; set; } = null!;


    public ICollection<Box> Boxes { get; set; } = [];
}
