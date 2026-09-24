namespace AerationSterilize.Application.Features.V1.BoxingJob.Common.Dtos;
public sealed record ScanWorkTableDto(int WorkTableSessionId, string WorkTableName, string WorkTableDescription, int WorkLineId);
