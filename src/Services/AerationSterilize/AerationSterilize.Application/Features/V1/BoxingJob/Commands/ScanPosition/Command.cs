using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanPosition;
public sealed record ScanPositionCommand(string PositionCode) : ICommand;

