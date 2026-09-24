using Contracts.Services.QRCode;
using QRCoder;

namespace Infrastructure.Services.QRCode;
public class QrCodeServices : IQrCodeServices
{

    public async Task<string> GenerateQrCodeAsync(string content, CancellationToken cancellationToken)
    {
        using var qrGenerator = new QRCodeGenerator();

        using var qrData = qrGenerator.CreateQrCode(
            content,
            QRCodeGenerator.ECCLevel.Q);

        var svgQr = new SvgQRCode(qrData);

        return svgQr.GetGraphic(1);

    }
}
