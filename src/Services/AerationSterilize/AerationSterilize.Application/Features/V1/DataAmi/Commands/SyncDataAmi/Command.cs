using Contracts.Common.Messages;
using Shared.Options;

namespace AerationSterilize.Application.Features.V1.DataAmi.Commands.SyncDataAmi;
public sealed record SyncDataAmiCommand(DataPlanOptions options) : ICommand;

