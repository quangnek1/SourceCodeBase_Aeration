using AerationSterilize.Domain.Entities.Working;
using AerationSterilize.Domain.Enums;
using Contracts.Abstractions.Entities.Domains;

namespace AerationSterilize.Domain.Entities.BoxingPositions;
public class BoxingPosition : EntityAuditBase<int>
{
    public string PositionCode { get; set; }
    public string? Image { get; set; }
    public PositionStatus? Status { get; set; }
    public int BoxingColumnId { get; set; }
    public BoxingColumn BoxingColumn { get; set; }

    public ICollection<Box> Boxes { get; set; }
}
