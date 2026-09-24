using AerationSterilize.Application.Features.V1.Products.Common.Dtos;

namespace AerationSterilize.Application.Abstractions;

public interface IProductSearchIndexService
{
    bool IsEnabled { get; }

    Task IndexAsync(ProductDto product, CancellationToken cancellationToken = default);

    Task DeleteAsync(Guid productId, CancellationToken cancellationToken = default);

    Task RebuildAsync(IEnumerable<ProductDto> products, CancellationToken cancellationToken = default);
}