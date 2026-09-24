using Contracts.Abstractions.Entities.Domains;

namespace AerationSterilize.Domain.Entities.Working;
public class WorkLine : EntityBase<int>
{
    public string Name { get; set; }
    public bool? Status { get; set; }

    public ICollection<WorkTable> WorkTables { get; set; }
}
