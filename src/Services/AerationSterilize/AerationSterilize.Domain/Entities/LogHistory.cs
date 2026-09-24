using Contracts.Abstractions.Entities.Domains;
using Contracts.Abstractions.Entities.Domains.Interfaces;

namespace AerationSterilize.Domain.Entities;
public class LogHistory : EntityAuditBase<int>, IUserTracking
{
    public int? BatchId { get; set; }
    public int? BatchItemId { get; set; }
    public string Action { get; set; } = null!;
    public string? Description { get; set; }
    public string? OldValue { get; set; }
    public string? NewValue { get; set; }

    // Audit
    public string? CreatedBy { get; set; }
    public string? LastModifiedBy { get; set; }
}
