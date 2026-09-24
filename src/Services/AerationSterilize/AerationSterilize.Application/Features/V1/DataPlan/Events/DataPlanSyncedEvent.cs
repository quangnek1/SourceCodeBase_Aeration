using Contracts.Common.Messages;

namespace AerationSterilize.Application.Features.V1.DataPlan.Events;
public sealed record DataPlanSyncedEvent(Guid Id) : IDomainEvent;

