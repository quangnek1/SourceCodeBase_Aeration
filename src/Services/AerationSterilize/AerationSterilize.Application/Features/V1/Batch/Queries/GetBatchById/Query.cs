using AerationSterilize.Application.Features.V1.Batch.Common.Dtos;
using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.Batch.Queries.GetBatchF6112;
public sealed record GetBatchByIdQuery(int Id) : IQuery<BatchDto>;

