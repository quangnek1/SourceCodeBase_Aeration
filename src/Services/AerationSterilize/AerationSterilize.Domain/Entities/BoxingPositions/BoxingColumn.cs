using Contracts.Abstractions.Entities.Domains;

namespace AerationSterilize.Domain.Entities.BoxingPositions;
public class BoxingColumn : EntityBase<int>
{
    public string ColumnName { get; set; }
    public bool? Status { get; set; }
    public ICollection<BoxingPosition> BoxingPositions { get; set; }
}
