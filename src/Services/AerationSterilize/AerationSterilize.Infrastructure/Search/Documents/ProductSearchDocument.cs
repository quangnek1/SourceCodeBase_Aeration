using AerationSterilize.Application.Features.V1.Products.Common.Dtos;

namespace AerationSterilize.Infrastructure.Search.Documents;

public sealed class ProductSearchDocument
{
    public Guid Id { get; init; }
    public string Name { get; init; } = string.Empty;
    public decimal Price { get; init; }
    public string Description { get; init; } = string.Empty;

    public static ProductSearchDocument FromDto(ProductDto product)
    {
        return new ProductSearchDocument
        {
            Id = product.Id,
            Name = product.Name,
            Price = product.Price,
            Description = product.Description
        };
    }

    public ProductDto ToDto()
    {
        return new ProductDto(Id, Name, Price, Description);
    }
}