using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.Batch.Commands.UpdateBatchStatus;
public sealed record UpdateBatchStatusCommand(int BatchId) : ICommand;

