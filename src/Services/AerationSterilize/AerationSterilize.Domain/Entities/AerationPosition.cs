using AerationSterilize.Domain.Enums;
using Contracts.Abstractions.Entities.Domains;

namespace AerationSterilize.Domain.Entities;
public class AerationPosition : EntityAuditBase<int>
{
    public string PositionCode { get; set; }
    public string? Image { get; set; }
    public AerationStatus? Status { get; set; }
    public int AerationColumnId { get; set; }
    public AerationColumn AerationColumn { get; set; }

    public ICollection<BatchInAerationPosition> BatchInAerationPositions { get; set; } = [];

}
