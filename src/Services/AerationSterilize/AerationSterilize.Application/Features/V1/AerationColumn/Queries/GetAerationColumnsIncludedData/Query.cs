using AerationSterilize.Application.Features.V1.AerationColumn.Common.Dtos;
using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.AerationColumn.Queries.GetAerationColumnsIncludedData;
public sealed record GetAerationColumnsIncludedDataQuery : IQuery<IReadOnlyList<AerationColumnIncludedDto>>;
