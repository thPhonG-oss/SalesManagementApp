using SalesManagement.WinUI.Models;
using System.Threading.Tasks;

namespace SalesManagement.WinUI.Services.Interfaces
{
    public interface ICustomerService
    {
        Task<CustomerListData?> GetCustomersAsync(int page = 1, int size = 20, string search = "");
        Task<bool> CreateCustomerAsync(Customer customer);
        Task<bool> UpdateCustomerAsync(int id, Customer customer);
        Task<bool> DeleteCustomerAsync(int id);
    }
}