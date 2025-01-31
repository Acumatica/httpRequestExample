using System;
using System.Net.Http;
using System.Threading;
using System.Threading.Tasks;

namespace GetValueFromAPIExample
{
    public class ExternalAPIService : IExternalAPIService
    {
        private readonly IHttpClientFactory _httpClientFactory;

        public ExternalAPIService(IHttpClientFactory factory)
        {
            _httpClientFactory = factory;
        }

        public async Task<string> GetDataFromApi(CancellationToken cancellationToken)
        {
            HttpClient client = _httpClientFactory.CreateClient();
            HttpResponseMessage response = await client.GetAsync("https://reqres.in/api/users", cancellationToken);
            response.EnsureSuccessStatusCode();

            return await response.Content.ReadAsStringAsync();
        }
    }
}
