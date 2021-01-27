using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Hosting;
using Xunit;

namespace TraderService.Tests
{
    public class Test1
    {
        const string gqlUri = "https://localhost:5001/gql";

        const string queryTempl = @"  
            query Traders
            {
                tradersQuery
                {
                    traders(sortBy: '!age') {
                        id
                        firstName
                        lastName
                        birthdate
                        email
                        password
                        isDeleted
                        cryptocurrencies {
                            id
                            currency
                            symbol
                        }
                    }
                }
            }";

        const string mutationTempl = @"
            mutation TraderMutation
            {
                traderMutation
                {
                    createTraders(
                      tradersInput: 
                      [
                        {
                            firstName: 'Lior'
                            lastName: 'Levy'
                            birthdate: '1950-01-01'
                            email: 'llevy@trader.com'
                            password: 'lll'
                            isDeleted: false
                            cryptocurrencies:[{ id: 0 }{ id: 1 }]
   	                    }
                        {
                            firstName: 'Ann'
                            lastName: 'Linders'
                            birthdate: '1980-01-01'
                            email: 'annl@trader.com'
                            password: 'lll'
                            isDeleted: false
                            cryptocurrencies:[{ id: 0 }{ id: 0 }]
   	                    }
                      ]
                    ) 
                    {
                        status
                        message
                    }
                }
            }";

        public Test1()
        {

        }

        private static string TemplateToString(string template)
            => template?.Replace("'", "\"");

        private static async Task<string> QueryGraphQL(string uri, string query)
        {
            var postData = new { Query = query };
            HttpClient httpClient = new();
            StringContent stringContent = new(JsonConvert.SerializeObject(postData), Encoding.UTF8, "application/json");
            var res = await httpClient.PostAsync(uri, stringContent);
            return res.IsSuccessStatusCode
                ? await res.Content.ReadAsStringAsync()
                : $"Error occurred... Status code:{res.StatusCode}";
        }

        [Fact]
        public void TestMethod1()
        {
            Console.WriteLine("TestMethod1");

            var query = TemplateToString(queryTempl);
            var mutation = TemplateToString(mutationTempl);

            Program.CreateHostBuilder(new string[0]).Build().Run();

            var result = QueryGraphQL(gqlUri, query).Result;
            //Task.Run(async () =>
            //{
            //    Console.WriteLine(result);
            //});


            //var primeService = new PrimeService();
            //bool result = primeService.IsPrime(1);

            //Assert.False(result, "1 should not be prime");
        }
    }
}
