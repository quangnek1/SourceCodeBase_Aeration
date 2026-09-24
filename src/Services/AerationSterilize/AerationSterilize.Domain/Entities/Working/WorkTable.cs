using Contracts.Abstractions.Entities.Domains;
using Shared.Emumerations;

namespace AerationSterilize.Domain.Entities.Working;
public class WorkTable : EntityBase<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public WorkTableStatus? Status { get; set; }

    public int WorkLineId { get; set; }
    public WorkLine WorkLine { get; set; }

}
