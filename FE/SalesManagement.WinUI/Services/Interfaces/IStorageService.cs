using SalesManagement.WinUI.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.Services.Interfaces
{
    public interface IStorageService
    {
        Task SaveCredentialsAsync(string username, string password, bool rememberMe);

        /// <summary>
        /// Retrieves stored credentials
        /// </summary>
        Task<StoredCredentials?> GetStoredCredentialsAsync();

        /// <summary>
        /// Clears stored credentials
        /// </summary>
        Task ClearCredentialsAsync();

        /// <summary>
        /// Checks if credentials are stored
        /// </summary>
        Task<bool> HasStoredCredentialsAsync();

        /// <summary>
        /// Saves app settings
        /// </summary>
        Task SaveSettingAsync(string key, string value);

        /// <summary>
        /// Gets app setting
        /// </summary>
        Task<string?> GetSettingAsync(string key);

        /// <summary>
        /// Saves last opened screen
        /// </summary>
        Task SaveLastScreenAsync(string screenName);

        /// <summary>
        /// Gets last opened screen
        /// </summary>
        Task<string?> GetLastScreenAsync();

        // ===== APP SETTINGS (MỚI) =====
        /// <summary>
        /// Lưu toàn bộ cấu hình app
        /// </summary>
        Task SaveAppSettingsAsync(AppSettings settings);

        /// <summary>
        /// Lấy toàn bộ cấu hình app
        /// </summary>
        Task<AppSettings> GetAppSettingsAsync();

        /// <summary>
        /// Reset về cấu hình mặc định
        /// </summary>
        Task ResetAppSettingsAsync();
    }
}
