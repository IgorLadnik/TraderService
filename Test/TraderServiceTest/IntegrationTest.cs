using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc.Testing;
using Newtonsoft.Json;
using TraderService;

namespace TraderServiceTest
{
    public class IntegrationTest
    {
        const string _gqlUri = "https://localhost:7001/gql";

        private readonly HttpClient _testClient;

        public IntegrationTest()
        {
            // In-memory service
            _testClient = new WebApplicationFactory<Startup>().CreateClient();
        }

        protected async Task<string> Execute(string query)
        {
            var postData = new { Query = Adjust(query) };
            StringContent stringContent = new(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");
            var response = await _testClient.PostAsync(_gqlUri, stringContent);
            return response.IsSuccessStatusCode
                ? await response.Content.ReadAsStringAsync()
                : $"Error. Status code: {response.StatusCode}";
        }

        protected static string Adjust(string query) => query?.Replace("'", "\"");
    }
}
