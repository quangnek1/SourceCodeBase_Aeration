namespace AerationSterilize.Application.Features.V1.AerationColumn.Common.Dtos;

public record AerationColumnDto(
    int Id,
    string ColumnName,
    bool? Status);
