using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.AerationPosition.Commands.DeleteAerationPosition;

public sealed record DeleteAerationPositionCommand(int Id) : ICommand;
