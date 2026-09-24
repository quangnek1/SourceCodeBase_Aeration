namespace Contracts.Services.PDF;
public interface ITemplateRenderer
{
    Task<string> RenderAsync<T>(string viewName, T model, string version = "V8");
}
