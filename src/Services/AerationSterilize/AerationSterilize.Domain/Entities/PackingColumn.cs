using Contracts.Abstractions.Entities.Domains;

namespace AerationSterilize.Domain.Entities;
public class PackingColumn : EntityBase<int>
{
    public string ColumnName { get; set; }
    public bool? Status { get; set; }
    public ICollection<PackingPosition> PackingPositions { get; set; }
}
