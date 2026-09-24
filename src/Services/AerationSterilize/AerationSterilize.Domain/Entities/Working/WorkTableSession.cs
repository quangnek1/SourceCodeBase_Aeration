using AerationSterilize.Domain.Entities.Identity;
using Contracts.Abstractions.Entities.Domains;

namespace AerationSterilize.Domain.Entities.Working;
public class WorkTableSession : EntityAuditBase<int>
{
    public Guid AppUserId { get; set; }
    public int WorkTableId { get; set; }

    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset? EndedAt { get; set; }
    public bool IsActive { get; set; }

    public WorkTable WorkTable { get; set; }
    public AppUser AppUser { get; set; }
}
