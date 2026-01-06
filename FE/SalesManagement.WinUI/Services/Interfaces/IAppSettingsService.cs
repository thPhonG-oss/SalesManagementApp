namespace SalesManagement.WinUI.Services.Interfaces
{
    public interface IAppSettingsService
    {
        int GetApiPort();
        void UpdateApiPort(int port);
        string GetBaseUrl();
    }

}
