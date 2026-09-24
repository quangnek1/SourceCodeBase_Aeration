using AerationSterilize.Domain.Entities.BoxingPositions;
using AerationSterilize.Domain.Entities.Identity;
using Contracts.Abstractions.Entities.Domains;

namespace AerationSterilize.Domain.Entities.Working;
public class BoxingSession : EntityAuditBase<int>
{
    public Guid AppUserId { get; set; }

    public int BoxingPositionId { get; set; }

    public int? BoxingJobId { get; set; }

    public int? CurrentBoxId { get; set; }

    public bool IsActive { get; set; }

    public DateTimeOffset StartedAt { get; set; }

    public DateTimeOffset? EndedAt { get; set; }

    public AppUser AppUser { get; set; } = null!;

    public BoxingPosition BoxingPosition { get; set; } = null!;

    public BoxingJob? BoxingJob { get; set; }

    public Box? CurrentBox { get; set; }
}
