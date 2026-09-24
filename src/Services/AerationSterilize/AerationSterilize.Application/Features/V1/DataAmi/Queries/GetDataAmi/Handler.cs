using AerationSterilize.Application.Features.V1.DataAmi.Common.Dtos;
using AerationSterilize.Application.Features.V1.DataPlan.Common.Dtos;
using AerationSterilize.Domain.Entities;
using Contracts.Common.Messages;
using Contracts.Common.Repositories;
using Contracts.Responses;
using Contracts.Services.Cache;
using Microsoft.EntityFrameworkCore;
using Shared.Common.Contants;
using Serilog;

namespace AerationSterilize.Application.Features.V1.DataAmi.Queries.GetDataAmi;
internal class GetDataAmiQueryHandler : IQueryHandler<GetDataAmiQuery, IEnumerable<DataAmiDto>>
{
    private readonly IRepositoryBase<DataAmiQ411, int> _repositoryBase;
    private readonly ICacheService _cacheService;
    private readonly ILogger _logger;
    private static readonly TimeSpan CacheExpiration = TimeSpan.FromHours(24);

    public GetDataAmiQueryHandler(IRepositoryBase<DataAmiQ411, int> repositoryBase, ILogger logger, ICacheService cacheService)
    {
        _repositoryBase = repositoryBase ?? throw new ArgumentNullException(nameof(repositoryBase));
        _logger = logger;
        _cacheService = cacheService ?? throw new ArgumentNullException(nameof(cacheService));
    }

    public async Task<Result<IEnumerable<DataAmiDto>>> Handle(GetDataAmiQuery request, CancellationToken ct)
    {
        var all = await GetAllFromCacheOrDbAsync(ct);
        var result = ApplyFilter(all, request.filter);

        return Result<IEnumerable<DataAmiDto>>.Success(result);
    }

    private async Task<List<DataAmiDto>> GetAllFromCacheOrDbAsync(CancellationToken ct)
    {
        // 1. Redis
        try
        {
            var cached = await _cacheService.GetAsync<List<DataAmiDto>>(RedisCacheKeys.DataAmiCacheKey, ct);

            if (cached is { Count: > 0 })
            {
                _logger.Information("Data-Ami retrieved from cache. Count: {Count}", cached.Count);
                return cached;
            }
        }
        catch (Exception ex)
        {
            // Redis lỗi -> fallback DB, không throw
            _logger.Warning(ex, "Cannot read Data-Ami from Redis. Fallback to database.");
        }

        // 2. Database (projection thẳng sang DTO -> nhẹ hơn load entity rồi map)
        var data = await _repositoryBase.FindAll(tracking: false)
            .OrderBy(x => x.ProductName)
            .Select(entity => new DataAmiDto
            {
                Id = entity.Id,
                ProductName = entity.ProductName,
                ProductInformation = entity.ProductInformation,
                DrawingNumber = entity.DrawingNumber,
                CatalogCode = entity.CatalogCode,
                Destination = entity.Destination,
                ChamberA = entity.ChamberA,
                ChamberB = entity.ChamberB,
                ChamberC = entity.ChamberC,
                ChamberD = entity.ChamberD,
                Status = entity.Status,
            })
            .ToListAsync(ct);

        _logger.Information("Data-Ami retrieved from database. Count: {Count}", data.Count);

        // 3. Ghi cache
        if (data.Count > 0)
        {
            try
            {
                await _cacheService.SetAsync(RedisCacheKeys.DataAmiCacheKey, data, CacheExpiration, ct);
            }
            catch (Exception ex)
            {
                _logger.Warning(ex, "Cannot write Data-Ami to Redis.");
            }
        }

        return data;
    }

    private static IEnumerable<DataAmiDto> ApplyFilter(List<DataAmiDto> source, string? filter)
    {
        if (string.IsNullOrWhiteSpace(filter))
            return source;

        var keyword = filter.Trim();

        return source.Where(x =>
            Match(x.ProductName, keyword) ||
            Match(x.ProductInformation, keyword) ||
            Match(x.DrawingNumber, keyword) ||
            Match(x.CatalogCode, keyword) ||
            Match(x.Destination, keyword))
            .ToList();

        static bool Match(string? value, string keyword)
            => !string.IsNullOrEmpty(value)
               && value.Contains(keyword, StringComparison.OrdinalIgnoreCase);
    }
}
