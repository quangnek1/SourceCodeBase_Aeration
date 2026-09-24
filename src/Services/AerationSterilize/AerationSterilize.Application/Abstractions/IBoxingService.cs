
using AerationSterilize.Domain.Entities.Working;

namespace AerationSterilize.Application.Abstractions;
public interface IBoxingService
{
    Task<Box> ValidateAndGetBoxAsync(string boxCode, CancellationToken cancellationToken);
}
