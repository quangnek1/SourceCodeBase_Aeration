using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanTag;
public sealed record ScanTagCommand(string QrCode) : ICommand;
