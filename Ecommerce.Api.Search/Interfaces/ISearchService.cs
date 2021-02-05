using System.Threading.Tasks;

namespace Ecommerce.Api.Search.Interfaces
{
    public interface ISearchService
    {
        Task<(bool isSuccess, dynamic SearchResults)> SearchAsync(int customerId);
    }
}
