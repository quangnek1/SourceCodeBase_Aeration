using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.AerationPosition.Commands.CreateAerationPosition;

public sealed record CreateAerationPositionCommand(
    string PositionCode,
    string? Image,
    int? Status,
    int AerationColumnId
) : ICommand<int>;
