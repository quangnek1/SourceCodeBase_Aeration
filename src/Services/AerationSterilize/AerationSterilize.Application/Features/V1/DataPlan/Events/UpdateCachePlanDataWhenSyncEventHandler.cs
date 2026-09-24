using Contracts.Common.Messages;
using Microsoft.Extensions.Caching.Distributed;
using Shared.Common.Contants;

namespace AerationSterilize.Application.Features.V1.DataPlan.Events;
internal class UpdateCachePlanDataWhenSyncEventHandler : IDomainEventHandler<DataPlanSyncedEvent>
{
    private readonly IDistributedCache _cache;

    public UpdateCachePlanDataWhenSyncEventHandler(IDistributedCache cache)
    {
        _cache = cache ?? throw new ArgumentNullException(nameof(cache));
    }

    public async Task Handle(DataPlanSyncedEvent notification, CancellationToken cancellationToken)
    {
        await _cache.RemoveAsync(RedisCacheKeys.DataPlanCacheKey);
    }

}
