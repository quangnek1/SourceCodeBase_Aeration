using Contracts.Abstractions.Entities.Domains;

namespace AerationSterilize.Domain.Entities;
public class Setting : EntityAuditBase<int>
{
    public string PlanCAG { get; set; }
    public string PlanPTCA { get; set; }
    public string DataAmiQ411 { get; set; }
}
