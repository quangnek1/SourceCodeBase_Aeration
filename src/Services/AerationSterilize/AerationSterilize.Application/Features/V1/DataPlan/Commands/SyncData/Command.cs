using Contracts.Common.Messages;
using Shared.Options;

namespace AerationSterilize.Application.Features.V1.DataPlan.Commands.SyncData;
public sealed record SyncDataCommand(DataPlanOptions options) : ICommand;

