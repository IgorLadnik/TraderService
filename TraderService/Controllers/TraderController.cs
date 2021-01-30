using System;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GraphQlHelperLib;
using GraphQL.Types;
using GraphQL;
using TraderModelLib.Data;
using RepoInterfaceLib;

namespace TraderService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class TraderController : GqlControllerBase
    {
        public TraderController(
            ISchema schema,
            IDocumentExecuter documentExecuter,
            IConfiguration configuration,
            ILogger<ControllerBase> logger)
                : base(schema, documentExecuter, configuration, logger)
        {
        }

        [HttpGet]
        public IActionResult About() => Ok($"TraderService was called at {DateTime.Now}");

        [HttpGet("{id}")]
        public async Task<IActionResult> Get(int id)
        {
            var query =
                @"   
                query TraderByUniqueProperty {
                    traderByPropertyQuery {
                        traderByUniqueProperty(id: /*id*/) {
                            id
                            isDeleted
                            firstName
                            lastName
                            cryptocurrencies {
                                id
                                currency
                                symbol
                            }
                        }
                    }
                }".Replace("/*id*/", $"{id}");

            return await ProcessQuery(query);
        }
    }
}
