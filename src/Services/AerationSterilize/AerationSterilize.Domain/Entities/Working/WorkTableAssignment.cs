using Contracts.Abstractions.Entities.Domains;

namespace AerationSterilize.Domain.Entities.Working;
public class WorkTableAssignment : EntityAuditBase<int>
{
    public int WorkTableId { get; set; }
    public int PackingPositionId { get; set; }

    public bool IsActive { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset? EndedAt { get; set; }

    public WorkTable WorkTable { get; set; }
    public PackingPosition PackingPosition { get; set; }
}
