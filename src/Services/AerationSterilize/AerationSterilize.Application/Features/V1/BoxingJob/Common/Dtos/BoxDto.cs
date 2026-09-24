using Shared.Emumerations;

namespace AerationSterilize.Application.Features.V1.BoxingJob.Common.Dtos;
public class BoxDto
{
    public string BoxCode { get; set; }
    public string INT { get; set; }
    public string? SEQ { get; set; }
    public int Capacity { get; set; }
    public BoxStatus? Status { get; set; }

    // Job mà Box này thuộc về
    public int? BoxingJobId { get; set; }
    public BoxingJobDto? BoxingJob { get; set; }

    public List<ItemTagDto>? ItemTag { get; set; }

    }
