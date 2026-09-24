namespace Shared.Options;

public class ElasticsearchOptions
{
    public bool Enabled { get; set; }
    public string Uri { get; set; } = "http://localhost:9200";
    public string? Username { get; set; }
    public string? Password { get; set; }
    public string? ApiKey { get; set; }
    public string DefaultIndex { get; set; } = "aeration-sterilize";
    public string ProductsIndex { get; set; } = "aeration-products";
}