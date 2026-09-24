using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.Batch.Events;
public sealed record UpdateCountPrintBatchWhenPrintPdfEvent(Guid Id, IReadOnlyList<int> BatchId) : IDomainEvent;
