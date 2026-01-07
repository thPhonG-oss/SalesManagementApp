using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;

namespace SalesManagement.WinUI.Services.Implementations;

public class StorageService : IStorageService
{
    private const string CredentialsFileName = "credentials.json";
    private const string SettingsFileName = "settings.json";
    private const string AppSettingsFileName = "app_settings.json";

    private readonly string _appFolderPath;

    public StorageService()
    {
        _appFolderPath = Path.Combine(
            Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData),
            "SalesManagement");

        Directory.CreateDirectory(_appFolderPath);
    }

    private string GetFilePath(string fileName)
        => Path.Combine(_appFolderPath, fileName);

    // ================= CREDENTIALS =================

    public async Task SaveCredentialsAsync(string username, string password, bool rememberMe)
    {
        if (!rememberMe)
        {
            await ClearCredentialsAsync();
            return;
        }

        var credentials = new StoredCredentials
        {
            Username = username,
            EncryptedPassword = EncryptPassword(password),
            RememberMe = rememberMe,
            LastLoginTime = DateTime.Now
        };

        var json = JsonSerializer.Serialize(credentials);
        await File.WriteAllTextAsync(GetFilePath(CredentialsFileName), json);
    }

    public async Task<StoredCredentials?> GetStoredCredentialsAsync()
    {
        var path = GetFilePath(CredentialsFileName);
        if (!File.Exists(path))
            return null;

        try
        {
            var json = await File.ReadAllTextAsync(path);
            var credentials = JsonSerializer.Deserialize<StoredCredentials>(json);

            if (credentials?.RememberMe == true)
            {
                credentials.EncryptedPassword =
                    DecryptPassword(credentials.EncryptedPassword);
                return credentials;
            }
        }
        catch { }

        return null;
    }

    public Task ClearCredentialsAsync()
    {
        var path = GetFilePath(CredentialsFileName);
        if (File.Exists(path))
            File.Delete(path);

        return Task.CompletedTask;
    }

    public Task<bool> HasStoredCredentialsAsync()
        => Task.FromResult(File.Exists(GetFilePath(CredentialsFileName)));

    // ================= SETTINGS =================

    public async Task SaveSettingAsync(string key, string value)
    {
        var settings = await LoadSettingsAsync();
        settings[key] = value;

        await File.WriteAllTextAsync(
            GetFilePath(SettingsFileName),
            JsonSerializer.Serialize(settings));
    }

    public async Task<string?> GetSettingAsync(string key)
    {
        var settings = await LoadSettingsAsync();
        return settings.TryGetValue(key, out var value) ? value : null;
    }

    public Task SaveLastScreenAsync(string screenName)
        => SaveSettingAsync("LastScreen", screenName);

    public Task<string?> GetLastScreenAsync()
        => GetSettingAsync("LastScreen");

    private async Task<Dictionary<string, string>> LoadSettingsAsync()
    {
        var path = GetFilePath(SettingsFileName);
        if (!File.Exists(path))
            return new Dictionary<string, string>();

        var json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<Dictionary<string, string>>(json)
               ?? new Dictionary<string, string>();
    }

    // ================= APP SETTINGS =================

    public async Task SaveAppSettingsAsync(AppSettings settings)
    {
        var json = JsonSerializer.Serialize(settings, new JsonSerializerOptions
        {
            WriteIndented = true
        });

        await File.WriteAllTextAsync(GetFilePath(AppSettingsFileName), json);
    }

    public async Task<AppSettings> GetAppSettingsAsync()
    {
        var path = GetFilePath(AppSettingsFileName);
        if (!File.Exists(path))
            return new AppSettings();

        var json = await File.ReadAllTextAsync(path);
        return JsonSerializer.Deserialize<AppSettings>(json) ?? new AppSettings();
    }

    public Task ResetAppSettingsAsync()
    {
        var path = GetFilePath(AppSettingsFileName);
        if (File.Exists(path))
            File.Delete(path);

        return Task.CompletedTask;
    }

    // ================= SECURITY =================

    private string EncryptPassword(string password)
    {
        try
        {
            var data = Encoding.UTF8.GetBytes(password);
            var encrypted = ProtectedData.Protect(data, null, DataProtectionScope.CurrentUser);
            return Convert.ToBase64String(encrypted);
        }
        catch
        {
            return password;
        }
    }

    private string DecryptPassword(string encryptedPassword)
    {
        try
        {
            var data = Convert.FromBase64String(encryptedPassword);
            var decrypted = ProtectedData.Unprotect(data, null, DataProtectionScope.CurrentUser);
            return Encoding.UTF8.GetString(decrypted);
        }
        catch
        {
            return encryptedPassword;
        }
    }
}
