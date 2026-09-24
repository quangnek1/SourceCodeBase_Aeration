using AerationSterilize.Application.Abstractions;
using AerationSterilize.Application.Features.V1.Products.Common.Dtos;
using AerationSterilize.Domain.Entities;
using Contracts.Common.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace AerationSterilize.Infrastructure.Search;

public sealed class ProductSearchRebuildJob : IProductSearchRebuildJob
{
    private readonly IRepositoryBase<Product, Guid> _productRepository;
    private readonly IProductSearchIndexService _productSearchIndexService;
    private readonly ILogger<ProductSearchRebuildJob> _logger;

    public ProductSearchRebuildJob(
        IRepositoryBase<Product, Guid> productRepository,
        IProductSearchIndexService productSearchIndexService,
        ILogger<ProductSearchRebuildJob> logger)
    {
        _productRepository = productRepository ?? throw new ArgumentNullException(nameof(productRepository));
        _productSearchIndexService = productSearchIndexService ?? throw new ArgumentNullException(nameof(productSearchIndexService));
        _logger = logger ?? throw new ArgumentNullException(nameof(logger));
    }

    public async Task RebuildProductsAsync(CancellationToken cancellationToken = default)
    {
        if (!_productSearchIndexService.IsEnabled)
        {
            _logger.LogInformation("Product search index rebuild skipped because Elasticsearch is disabled.");
            return;
        }

        var products = await _productRepository
            .FindAll()
            .Select(product => new ProductDto(
                product.Id,
                product.Name,
                product.Price,
                product.Description))
            .ToListAsync(cancellationToken);

        await _productSearchIndexService.RebuildAsync(products, cancellationToken);

        _logger.LogInformation("Product search index rebuild completed. Products indexed: {ProductCount}", products.Count);
    }
}