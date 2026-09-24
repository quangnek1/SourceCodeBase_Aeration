namespace AerationSterilize.Application.Abstractions;

public interface IProductSearchRebuildJob
{
    Task RebuildProductsAsync(CancellationToken cancellationToken = default);
}