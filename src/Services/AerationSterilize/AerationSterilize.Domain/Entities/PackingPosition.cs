using AerationSterilize.Domain.Entities.Working;
using AerationSterilize.Domain.Enums;
using Contracts.Abstractions.Entities.Domains;

namespace AerationSterilize.Domain.Entities;
public class PackingPosition : EntityAuditBase<int>
{
    public string PositionCode { get; set; }
    public string? Image { get; set; }
    public AerationStatus? Status { get; set; }
    public int PackingColumnId { get; set; }
    public PackingColumn PackingColumn { get; set; }

    public ICollection<Box> Boxes { get; set; }
}
