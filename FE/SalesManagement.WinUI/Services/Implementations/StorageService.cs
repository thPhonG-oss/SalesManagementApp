using SalesManagement.WinUI.Models;
using SalesManagement.WinUI.Services.Interfaces;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.Json;
using Windows.Storage;

namespace SalesManagement.WinUI.Services.Implementations;

public class StorageService : IStorageService
{
    private const string CredentialsFileName = "credentials.dat";
    private const string SettingsFileName = "settings.json";
    private readonly ApplicationDataContainer _localSettings;
    private readonly StorageFolder _localFolder;

    public StorageService()
    {
        _localSettings = ApplicationData.Current.LocalSettings;
        _localFolder = ApplicationData.Current.LocalFolder;
    }

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
        var file = await _localFolder.CreateFileAsync(CredentialsFileName, CreationCollisionOption.ReplaceExisting);
        await FileIO.WriteTextAsync(file, json);
    }

    public async Task<StoredCredentials?> GetStoredCredentialsAsync()
    {
        try
        {
            var file = await _localFolder.GetFileAsync(CredentialsFileName);
            var json = await FileIO.ReadTextAsync(file);
            var credentials = JsonSerializer.Deserialize<StoredCredentials>(json);

            if (credentials != null && credentials.RememberMe)
            {
                // Decrypt password
                credentials.EncryptedPassword = DecryptPassword(credentials.EncryptedPassword);
                return credentials;
            }
        }
        catch (FileNotFoundException)
        {
            // File doesn't exist
        }
        catch (Exception ex)
        {
            System.Diagnostics.Debug.WriteLine($"Error loading credentials: {ex.Message}");
        }

        return null;
    }

    public async Task ClearCredentialsAsync()
    {
        try
        {
            var file = await _localFolder.GetFileAsync(CredentialsFileName);
            await file.DeleteAsync();
        }
        catch (FileNotFoundException)
        {
            // Already cleared
        }
    }

    public async Task<bool> HasStoredCredentialsAsync()
    {
        try
        {
            await _localFolder.GetFileAsync(CredentialsFileName);
            return true;
        }
        catch (FileNotFoundException)
        {
            return false;
        }
    }

    public Task SaveSettingAsync(string key, string value)
    {
        _localSettings.Values[key] = value;
        return Task.CompletedTask;
    }

    public Task<string?> GetSettingAsync(string key)
    {
        if (_localSettings.Values.TryGetValue(key, out var value))
        {
            return Task.FromResult(value?.ToString());
        }
        return Task.FromResult<string?>(null);
    }

    public Task SaveLastScreenAsync(string screenName)
    {
        return SaveSettingAsync("LastScreen", screenName);
    }

    public Task<string?> GetLastScreenAsync()
    {
        return GetSettingAsync("LastScreen");
    }

    // Simple encryption using Data Protection API
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
            return password; // Fallback
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
            return encryptedPassword; // Fallback
        }
    }
}