using Contracts.Abstractions.Entities.Domains;
using Shared.Emumerations;

namespace AerationSterilize.Domain.Entities;
public class Batch : EntityAuditBase<int>
{
    public string QRCode { get; set; }
    public DateTimeOffset SterilizeDate { get; set; }
    public string BatchNo { get; set; }
    public int Keep { get; set; }
    public int Print { get; set; }
    public DataStatus? Status { get; set; }

    // Aeration
    public DateTimeOffset? InputAerationActual { get; set; }
    public DateTimeOffset? PlanOutputAeration { get; set; }
    public DateTimeOffset? ActualOutputAeration { get; set; }
    public string? HangfireJobId { get; set; }

    public ICollection<BatchInAerationPosition> BatchInAerationPositions { get; set; }
    public ICollection<BatchItem> BatchItems { get; set; }
}
