using Microsoft.Extensions.Configuration;
using SalesManagement.WinUI.Services.Interfaces;
using System.IO;
using System.Text.Json;

public class AppSettingsService : IAppSettingsService
{
    private readonly string _configPath;
    private IConfigurationRoot _config;

    public AppSettingsService()
    {
        var appFolder = AppContext.BaseDirectory;
        _configPath = Path.Combine(appFolder, "appsettings.json");

        _config = new ConfigurationBuilder()
            .AddJsonFile(_configPath, optional: false, reloadOnChange: true)
            .Build();
    }

    public int GetApiPort()
    {
        var uri = new Uri(_config["ApiSettings:BaseUrl"]!);
        return uri.Port;
    }

    public string GetBaseUrl()
    {
        return _config["ApiSettings:BaseUrl"]!;
    }

    public void UpdateApiPort(int port)
    {
        var json = File.ReadAllText(_configPath);
        using var doc = JsonDocument.Parse(json);

        var root = doc.RootElement.Clone();
        var oldBaseUrl = root
            .GetProperty("ApiSettings")
            .GetProperty("BaseUrl")
            .GetString()!;

        var newBaseUrl = $"http://localhost:{port}";

        var updatedJson = json.Replace(oldBaseUrl, newBaseUrl);

        File.WriteAllText(_configPath, updatedJson);

        _config.Reload();
    }
}
