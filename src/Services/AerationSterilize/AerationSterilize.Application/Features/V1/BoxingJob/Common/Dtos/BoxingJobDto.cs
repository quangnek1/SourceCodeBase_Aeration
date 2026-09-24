using AerationSterilize.Domain.Entities;
using AerationSterilize.Domain.Entities.BoxingPositions;
using AerationSterilize.Domain.Entities.Working;
using Shared.Emumerations;

namespace AerationSterilize.Application.Features.V1.BoxingJob.Common.Dtos;
public class BoxingJobDto
{
    public int Id { get; set; }
    public int BoxingPositionId { get; set; }
    public BoxingPosition BoxingPosition { get; set; }

    public int TargetQty { get; set; }
    public BoxingJobStatus Status { get; set; }
    public DateTimeOffset StartedAt { get; set; }
    public DateTimeOffset? FinishedAt { get; set; }

    // 1 Job = 1 BatchItem = 1 Lot
    public BatchItem BatchItem { get; set; } = null!;


    public ICollection<Box> Boxes { get; set; } = [];
}
