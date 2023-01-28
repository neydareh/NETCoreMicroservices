using System.Collections.Generic;

namespace Ecommerce.Api.Search.Response {
  public class SearchResponse {
    public Models.Customer Customer { get; set; }
    public IEnumerable<Models.Order> Orders;
  }
}