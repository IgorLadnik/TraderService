using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Logging;
using GraphQL;
using GraphQL.Types;

namespace GraphQlHelperLib
{
    public class GqlControllerBase : ControllerBase
    {
        protected ILogger<ControllerBase> Logger { get; private set; }
        protected GraphqlProcessor Gql { get; private set; }

        protected GqlControllerBase(
            ISchema schema, 
            IDocumentExecuter documentExecuter, 
            IConfiguration configuration, 
            ILogger<ControllerBase> logger)
        {
            Logger = logger;
            Gql = new(schema, documentExecuter, configuration, logger);
        }

        protected async Task<IActionResult> ProcessQuery(GraphqlQuery query)
        {
            var result = await Gql.Process(query, User);
            return TransformResult(result) ?? Ok(result.Data);
        }

        protected async Task<IActionResult> ProcessQuery(string query)
        {
            var result = await Gql.Process(query, User);
            return TransformResult(result) ?? Ok(result.Data);
        }

        protected IActionResult TransformResult(ExecutionResult er)
        {
            if (er.Errors?.Count > 0)
            {
                var res = BadRequest(er);
                res.StatusCode = 400;
                res.Value = er.Errors[0].InnerException.Message;

                foreach (var err in er.Errors)
                    Logger.LogError(err.GetBaseException(), err.InnerException.Message);

                return res;
            }

            return null;
        }
    }
}

