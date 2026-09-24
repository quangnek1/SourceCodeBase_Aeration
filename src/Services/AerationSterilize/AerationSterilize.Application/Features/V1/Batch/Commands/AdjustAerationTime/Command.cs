using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.Batch.Commands.AdjustAerationTime;
public sealed record AdjustAerationTimeCommand(IReadOnlyList<int> BatchId, int AerationTime) : ICommand;
