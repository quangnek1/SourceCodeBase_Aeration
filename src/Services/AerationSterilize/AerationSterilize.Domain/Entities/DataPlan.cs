using Contracts.Abstractions.Entities.Domains;
using Shared.Emumerations;

namespace AerationSterilize.Domain.Entities;
public class DataPlan : EntityAuditBase<int>
{
    public string? PONo { get; set; }
    public string? TT { get; set; }
    public string? MaterialTypeC { get; set; }
    public string? PldOrd { get; set; }
    public string Material { get; set; }
    public string? ItemCodeTypeC { get; set; }
    public string? ItemCode { get; set; }
    public string? DrawingListNo { get; set; }
    public string? ItemName { get; set; }
    public string? InternalLot { get; set; }
    public string? Phase { get; set; }
    public string? ExternalLot1 { get; set; }
    public int? Qty { get; set; }
    public int? QtyInput { get; set; }
    public int KeepAeration { get; set; }
    public DateTime? CompleteDate { get; set; }
    public int? QATest { get; set; }
    public string? Destination { get; set; }
    public DateTime? Label { get; set; }
    public string? Bioburden { get; set; }
    public DateTime? Seal { get; set; }
    public DateTime? Ster { get; set; }
    public string? ME { get; set; }
    public string? BOXING { get; set; }
    public string? TestEndotoxin { get; set; }
    public string? TestParticle { get; set; }
    public double? LabelData { get; set; }
    public double? SealData { get; set; }
    public double? BoxingData { get; set; }
    public DateTime? ETD { get; set; }
    public string? Family { get; set; }
    public int? KeepStorage { get; set; }
    public string? MEChia { get; set; }
    public string? Type { get; set; }


    public DataStatus? Status { get; set; }

}
