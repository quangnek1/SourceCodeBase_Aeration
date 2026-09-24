using AerationSterilize.Application.Features.V1.DataPlan.Common.Dtos;
using AutoMapper;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Contracts.Services.Cache;
using Microsoft.EntityFrameworkCore;
using Serilog;
using Shared.Common.Contants;

namespace AerationSterilize.Application.Features.V1.DataPlan.Queries.GetAllData;
internal class GetAllDataQueryHandler : IQueryHandler<GetAllDataQuery, IReadOnlyList<DataPlanDto>>
{
    private readonly IRepositoryBase<Domain.Entities.DataPlan, int> _dataPlanRepository;
    private readonly IMapper _mapper;
    private readonly ICacheService _cacheService;
    private readonly ILogger _logger;
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromHours(24);

    public GetAllDataQueryHandler(
        IRepositoryBase<Domain.Entities.DataPlan, int> dataPlanRepository,
        IMapper mapper,
        ICacheService cacheService,
        ILogger logger)
    {
        _dataPlanRepository = dataPlanRepository ?? throw new ArgumentNullException(nameof(dataPlanRepository));
        _mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task<Result<IReadOnlyList<DataPlanDto>>> Handle(GetAllDataQuery request, CancellationToken cancellationToken)
    {
        // 1. Thử lấy dữ liệu từ Redis Cache
        var cachedData = await _cacheService.GetAsync<IReadOnlyList<DataPlanDto>>(RedisCacheKeys.DataPlanCacheKey, cancellationToken);
        if (cachedData is not null)
        {
            _logger.Information("DataPlan retrieved from cache.");
        }
        else
        {
            // 2. Cache trống — lấy từ Database
            var entities = await _dataPlanRepository
                .FindAll(tracking: false)
                .ToListAsync(cancellationToken);

            cachedData = _mapper.Map<IReadOnlyList<DataPlanDto>>(entities);

            // 3. Lưu toàn bộ data vào Cache để dùng cho các request tiếp theo
            if (cachedData.Count > 0)
                await _cacheService.SetAsync(RedisCacheKeys.DataPlanCacheKey, cachedData, CacheExpiration, cancellationToken);
        }

        // 4. Filter trên memory — dùng chung cho cả 2 nhánh
        var result = string.IsNullOrWhiteSpace(request.filter)
            ? cachedData
            : cachedData.Where(x => x.Type == request.filter).ToList();

        return Result<IReadOnlyList<DataPlanDto>>.Success(result);
    }
}
