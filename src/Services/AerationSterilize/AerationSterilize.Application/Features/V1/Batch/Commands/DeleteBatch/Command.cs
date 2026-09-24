using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.Batch.Commands.DeleteBatch;
public sealed record DeleteBatchCommand(int BatchId) : ICommand;
