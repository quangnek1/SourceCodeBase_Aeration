using AerationSterilize.Application.Features.V1.AerationColumn.Common.Dtos;
using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.AerationColumn.Queries.GetAerationColumnById;

public record GetAerationColumnByIdQuery(int Id) : IQuery<AerationColumnDto>;
