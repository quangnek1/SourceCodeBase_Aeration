using AerationSterilize.Application.Features.V1.DataPlan.Common.Dtos;
using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.DataPlan.Queries.GetAllData;
public sealed record GetAllDataQuery(string? filter) : IQuery<IReadOnlyList<DataPlanDto>>;

