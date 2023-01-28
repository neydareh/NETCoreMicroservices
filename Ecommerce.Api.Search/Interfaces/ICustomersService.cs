using System.Collections.Generic;
using System.Threading.Tasks;

namespace Ecommerce.Api.Search.Interfaces
{
  public interface ICustomersService
  {
    Task<(bool IsSuccess, Models.Customer Customer, string ErrorMessage)> GetCustomerAsync(int id);
    Task<(bool isSuccess, IEnumerable<Models.Customer> Customers, string errorMessage)> GetCustomersAsync();
  }
}
