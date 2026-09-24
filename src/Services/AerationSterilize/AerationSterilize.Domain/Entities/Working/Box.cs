using AerationSterilize.Domain.Entities.BoxingPositions;
using Contracts.Abstractions.Entities.Domains;
using Shared.Emumerations;

namespace AerationSterilize.Domain.Entities.Working;

public class Box : EntityAuditBase<int>
{
    public string BoxCode { get; set; }
    public string INT { get; set; }
    public string? SEQ { get; set; }
    public int Capacity { get; set; }
    public BoxStatus? Status { get; set; }

    // Job mà Box này thuộc về
    public int? BoxingJobId { get; set; }
    public BoxingJob? BoxingJob { get; set; }

    // tạo ở bàn đóng nào
    public int? BoxingPositionId { get; set; }
    public BoxingPosition? BoxingPosition { get; set; }

    // sau khi đầy chuyển sang keep
    public int? PackingPositionId { get; set; }
    public PackingPosition? PackingPosition { get; set; }

    // Lot đang đóng
    public int? BatchItemId { get; set; }
    public BatchItem? BatchItem { get; set; }

    public ICollection<ItemTag> ItemTags { get; set; }

}
