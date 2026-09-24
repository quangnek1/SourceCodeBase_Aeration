using AerationSterilize.Application.Features.V1.BoxingJob.Common.Dtos;
using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanBoxingPosition;
public sealed record ScanBoxingPositionCommand(string PositionCode) : ICommand<ScanBoxingPositionDto>;
 
