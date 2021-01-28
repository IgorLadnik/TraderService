using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GraphQL;
using GraphQL.Types;
using GraphQlHelperLib;

namespace TraderService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GqlController : GqlControllerBase
    {
        public GqlController(
            ISchema schema, 
            IDocumentExecuter documentExecuter, 
            IConfiguration configuration, 
            ILogger<ControllerBase> logger)
                : base(schema, documentExecuter, configuration, logger)
        {
        }

        [HttpPost]
        public async Task<IActionResult> PostAsyncAuth([FromBody] GraphqlQuery query) =>
            await ProcessQuery(query);
    }
}
