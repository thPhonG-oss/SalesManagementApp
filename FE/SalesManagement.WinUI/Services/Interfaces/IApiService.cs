using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.Services.Interfaces
{
    public interface IApiService
    {
        Task<T> GetAsync<T>(string endpoint);
        //Task<T> PostAsync<T>(string endpoint, object data);
        //Task<T> PutAsync<T>(string endpoint, object data);
        //Task<bool> DeleteAsync(string endpoint);
        //void SetAuthToken(string token);
        //void SetAuthHeader();
    }
}
