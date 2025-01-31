using System.Threading;
using System.Threading.Tasks;

namespace GetValueFromAPIExample
{
    public interface IExternalAPIService
    {
        Task<string> GetDataFromApi(CancellationToken cancellationToken);
    }
}
