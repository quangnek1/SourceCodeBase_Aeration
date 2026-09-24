using Contracts.Abstractions.Entities.Domains;

namespace AerationSterilize.Domain.Entities;
public class BatchInAerationPosition : EntityAuditBase<int>
{
    public int BatchId { get; set; }
    public int AerationPositionId { get; set; }

    public Batch Batch { get; set; }
    public AerationPosition AerationPosition { get; set; }

}
