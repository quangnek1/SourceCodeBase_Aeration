namespace Contracts.Services.PDF;
public interface IPdfService
{
    Task<byte[]> GeneratePDFAsync(string html,CancellationToken cancellationToken);
}
