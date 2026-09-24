using AerationSterilize.Application.Abstractions;
using AerationSterilize.Application.Features.V1.Products.Common.Dtos;
using AerationSterilize.Infrastructure.Search.Documents;
using Elastic.Clients.Elasticsearch;
using Microsoft.Extensions.Logging;
using Shared.Options;
using Shared.Paging;
using SharedSortOrder = Shared.Emumerations.SortOrder;

namespace AerationSterilize.Infrastructure.Search;

public sealed class ProductSearchService : IProductSearchService, IProductSearchIndexService
{
    private readonly ElasticsearchClient _client;
    private readonly ElasticsearchOptions _options;
    private readonly ILogger<ProductSearchService> _logger;

    public ProductSearchService(
        ElasticsearchClient client,
        ElasticsearchOptions options,
        ILogger<ProductSearchService> logger)
    {
        _client = client;
        _options = options;
        _logger = logger;
    }

    public bool IsEnabled => _options.Enabled && !string.IsNullOrWhiteSpace(_options.Uri);

    public async Task<PagedResult<ProductDto>> SearchAsync(
        string? searchTerm,
        string? sortColumn,
        SharedSortOrder? sortOrder,
        int pageIndex,
        int pageSize,
        CancellationToken cancellationToken = default)
    {
        pageIndex = Math.Max(pageIndex, PagedResult<ProductDto>.DefaultPageIndex);
        pageSize = Math.Clamp(pageSize, 1, PagedResult<ProductDto>.UpperPageSize);
        var from = (pageIndex - 1) * pageSize;
        var normalizedSearchTerm = searchTerm?.Trim();

        var response = await _client.SearchAsync<ProductSearchDocument>(search => search
            .Index(_options.ProductsIndex)
            .From(from)
            .Size(pageSize)
            .Query(query =>
            {
                if (string.IsNullOrWhiteSpace(normalizedSearchTerm))
                {
                    query.MatchAll(matchAll => matchAll.Boost(1));
                    return;
                }

                query.Bool(boolean => boolean
                    .Should(
                        should => should.Match(match => match.Field(product => product.Name).Query(normalizedSearchTerm)),
                        should => should.Match(match => match.Field(product => product.Description).Query(normalizedSearchTerm)))
                    .MinimumShouldMatch(1));
            }), cancellationToken);

        if (!response.IsValidResponse)
        {
            _logger.LogWarning("Elasticsearch product search failed: {Error}", response.DebugInformation);
            return Empty(pageIndex, pageSize);
        }

        var items = response.Documents.Select(product => product.ToDto()).ToList();
        var totalCount = Convert.ToInt32(response.Total);

        return new PagedResult<ProductDto>
        {
            Items = items,
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = totalCount
        };
    }

    public async Task IndexAsync(ProductDto product, CancellationToken cancellationToken = default)
    {
        if (!IsEnabled)
        {
            return;
        }

        var document = ProductSearchDocument.FromDto(product);
        var response = await _client.IndexAsync(document, index => index
            .Index(_options.ProductsIndex)
            .Id(product.Id), cancellationToken);

        if (!response.IsValidResponse)
        {
            _logger.LogWarning("Elasticsearch product index failed for {ProductId}: {Error}", product.Id, response.DebugInformation);
        }
    }

    public async Task DeleteAsync(Guid productId, CancellationToken cancellationToken = default)
    {
        if (!IsEnabled)
        {
            return;
        }

        var response = await _client.DeleteAsync<ProductSearchDocument>(productId, delete => delete
            .Index(_options.ProductsIndex), cancellationToken);

        if (!response.IsValidResponse)
        {
            _logger.LogWarning("Elasticsearch product delete failed for {ProductId}: {Error}", productId, response.DebugInformation);
        }
    }

    public async Task RebuildAsync(IEnumerable<ProductDto> products, CancellationToken cancellationToken = default)
    {
        if (!IsEnabled)
        {
            return;
        }

        var documents = products.Select(ProductSearchDocument.FromDto).ToArray();
        if (documents.Length == 0)
        {
            return;
        }

        var response = await _client.BulkAsync(bulk => bulk
            .Index(_options.ProductsIndex)
            .IndexMany(documents, (operation, document) => operation.Id(document.Id)), cancellationToken);

        if (!response.IsValidResponse)
        {
            _logger.LogWarning("Elasticsearch product rebuild failed: {Error}", response.DebugInformation);
        }
    }

    private static PagedResult<ProductDto> Empty(int pageIndex, int pageSize)
    {
        return new PagedResult<ProductDto>
        {
            Items = [],
            PageIndex = pageIndex,
            PageSize = pageSize,
            TotalCount = 0
        };
    }
}