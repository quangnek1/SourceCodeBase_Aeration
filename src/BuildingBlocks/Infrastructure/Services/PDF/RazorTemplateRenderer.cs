using Contracts.Services.PDF;
using RazorLight;

namespace Infrastructure.Services.PDF;
public class RazorTemplateRenderer : ITemplateRenderer
{
    private readonly RazorLightEngine _engine;

    public RazorTemplateRenderer()
    {
        var templateRoot = Path.Combine(AppContext.BaseDirectory,
            "DTOs", "PDF", "Templates");

        _engine = new RazorLightEngineBuilder()
            .UseFileSystemProject(templateRoot)
            .UseMemoryCachingProvider()
            .Build();
    }

    public async Task<string> RenderAsync<T>(string viewName, T model, string version = "V8")
    {
        string templatePath = $"{viewName}.cshtml";
        return await _engine.CompileRenderAsync(templatePath, model);
    }
}
