
using Contracts.Abstractions.Entities.Domains;

namespace AerationSterilize.Domain.Entities;
public class AerationColumn : EntityBase<int>
{
    public string ColumnName { get; set; }
    public bool? Status { get; set; }
    public ICollection<AerationPosition> AerationPositions { get; set; }
}
