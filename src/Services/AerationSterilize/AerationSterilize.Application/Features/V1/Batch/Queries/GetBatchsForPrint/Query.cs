using AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.Batch.Queries.GetBatchsForPrint;
public sealed record GetBatchsForPrintQuery(IReadOnlyList<int> BatchIds) : IQuery<List<BatchDto>>;
