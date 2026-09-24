using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanWorkTable;
public sealed record ScanWorkTableCommand(string TableCode) : ICommand;

