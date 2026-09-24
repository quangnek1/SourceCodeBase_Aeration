using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.AerationColumn.Commands.DeleteAerationColumn;

public sealed record DeleteAerationColumnCommand(int Id) : ICommand;
