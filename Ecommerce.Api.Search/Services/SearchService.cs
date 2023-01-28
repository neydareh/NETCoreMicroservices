using Ecommerce.Api.Search.Interfaces;
using Ecommerce.Api.Search.Response;
using System.Linq;
using System.Threading.Tasks;

namespace Ecommerce.Api.Search.Services
{
  public class SearchService : ISearchService
  {
    private readonly IOrdersService ordersService;
    private readonly IProductsService productsService;
    private readonly ICustomersService customersService;

    public SearchService(IOrdersService ordersService, IProductsService productsService, ICustomersService customersService)
    {
      this.ordersService = ordersService;
      this.productsService = productsService;
      this.customersService = customersService;
    }
    public async Task<(bool isSuccess, dynamic SearchResults)> SearchAsync(int customerId)
    {
      var ordersResult = await ordersService.GetOrdersAsync(customerId);
      var productsResult = await productsService.GetProductsAsync();
      var customersResult = await customersService.GetCustomerAsync(customerId);

      if (ordersResult.IsSuccess)
      {
        foreach (var order in ordersResult.Orders)
        {
          foreach (var item in order.Items)
          {
            item.ProductName = productsResult.IsSuccess ?
                productsResult.Products.FirstOrDefault(p => p.Id == item.ProductId)?.Name :
                "Product information is not available";
          }
        }

        var result = new SearchResponse();
        result.Customer = customersResult.IsSuccess ? customersResult.Customer : new Models.Customer{ Name = "Customer Not Found!"};
        result.Orders = ordersResult.Orders;

        System.Console.WriteLine("result" , result);
        
        return (true, result);
      }
      return (false, null);
    }
  }
}
