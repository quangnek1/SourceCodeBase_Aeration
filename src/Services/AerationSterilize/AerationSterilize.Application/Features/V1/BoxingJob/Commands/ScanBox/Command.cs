using AerationSterilize.Application.Features.V1.BoxingJob.Common.Dtos;
using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.BoxingJob.Commands.ScanBox;
public sealed record ScanBoxCommand(string BoxCode) : ICommand<ScanBoxDto>;
