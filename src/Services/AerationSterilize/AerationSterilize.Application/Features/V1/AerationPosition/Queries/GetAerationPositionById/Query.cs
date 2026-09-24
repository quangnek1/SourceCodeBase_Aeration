using AerationSterilize.Application.Features.V1.AerationPosition.Common.Dtos;
using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.AerationPosition.Queries.GetAerationPositionById;

public record GetAerationPositionByIdQuery(int Id) : IQuery<AerationPositionDto>;
