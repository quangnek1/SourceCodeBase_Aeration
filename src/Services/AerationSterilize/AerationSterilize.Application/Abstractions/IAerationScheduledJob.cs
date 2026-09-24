namespace AerationSterilize.Application.Abstractions;

public interface IAerationScheduledJob
{
    Task ExecuteOutputAerationAsync(int batchId);
}
