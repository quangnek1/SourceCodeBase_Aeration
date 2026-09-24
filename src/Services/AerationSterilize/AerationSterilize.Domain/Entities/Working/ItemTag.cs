using Contracts.Abstractions.Entities.Domains;

namespace AerationSterilize.Domain.Entities.Working;
public class ItemTag : EntityAuditBase<int>
{
    public string ItemcD { get; set; }
    public string INT { get; set; }
    public int Qty { get; set; }
    public string? Ext1 { get; set; }
    public string? Ext2 { get; set; }
    public string SEQNo { get; set; }
    public int? Type { get; set; }
    public bool? Status { get; set; }

    public int? BoxId { get; set; }
    public Box? Box { get; set; }
}
