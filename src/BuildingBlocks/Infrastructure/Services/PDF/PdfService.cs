using Contracts.Services.PDF;
using PuppeteerSharp;
using PuppeteerSharp.Media;

namespace Infrastructure.Services.PDF;
public class PdfService : IPdfService, IAsyncDisposable
{
    private volatile IBrowser? _browser;
    private readonly SemaphoreSlim _lock = new(1, 1);

    private async Task<IBrowser> GetBrowserAsync()
    {
        if (_browser is not null) return _browser;

        await _lock.WaitAsync();
        try
        {
            if (_browser is not null) return _browser;

            await new BrowserFetcher().DownloadAsync();
            _browser = await Puppeteer.LaunchAsync(new LaunchOptions { Headless = true });
            return _browser;
        }
        finally
        {
            _lock.Release();
        }
    }

    public async Task<byte[]> GeneratePDFAsync(string html, CancellationToken cancellationToken)
    {
        var browser = await GetBrowserAsync();

        await using var page = await browser.NewPageAsync();

        await page.SetContentAsync(html);

        return await page.PdfDataAsync(new PdfOptions
        {
            Format = PaperFormat.A4,
            Landscape = true,
            PrintBackground = true,
            PreferCSSPageSize = true,
            MarginOptions = new MarginOptions
            {
                Top = "5mm",
                Bottom = "5mm",
                Left = "5mm",
                Right = "5mm"
            }
        });
    }

    public async ValueTask DisposeAsync()
    {
        if (_browser is not null)
            await _browser.DisposeAsync();

        _lock.Dispose();

        GC.SuppressFinalize(this);
    }
}
