namespace Contracts.Services.QRCode;
public interface IQrCodeServices
{
    Task<string> GenerateQrCodeAsync(string content, CancellationToken cancellationToken);
}
