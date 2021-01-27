using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using GraphQL;
using GraphQL.NewtonsoftJson;
using GraphQL.Types;

namespace GraphQlHelperLib
{
    public class GraphqlProcessor
    {
        protected readonly IDocumentExecuter _documentExecuter;
        protected readonly ISchema _schema;
        protected readonly bool _isAuthJwt;

        public GraphqlProcessor(ISchema schema, IDocumentExecuter documentExecuter, IConfiguration configuration)
        {
            _schema = schema;
            _documentExecuter = documentExecuter;
            _isAuthJwt = configuration.GetValue<bool>("General:IsAuthJwt");
        }

        public async Task<ExecutionResult> Process(GraphqlQuery query, ClaimsPrincipal user)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            ////TEST
            //if (!query.IsIntrospection)
            //{
            //}

            var executionOptions = new ExecutionOptions
            {
                Query = query.Query,
                Inputs = query.Variables.ToInputs(),
            };

            return await SetParamsAndExecute(executionOptions, user);
        }

        public async Task<ExecutionResult> Process(string query, ClaimsPrincipal user)
        {
            if (string.IsNullOrEmpty(query))
                throw new ArgumentNullException(nameof(query));

            var executionOptions = new ExecutionOptions
            {
                Query = query,
            };

            return await SetParamsAndExecute(executionOptions, user);
        }

        private async Task<ExecutionResult> SetParamsAndExecute(ExecutionOptions executionOptions, ClaimsPrincipal user)
        {
            executionOptions.Schema = _schema;
            executionOptions.SetUser(user);
            return await _documentExecuter.ExecuteAsync(executionOptions);
        }
    }

    public class GraphqlQuery
    {
        public string OperationName { get; set; }
        public string NamedQuery { get; set; }
        public string Query { get; set; }
        public JObject Variables { get; set; }

        public bool IsIntrospection 
        {
            get => this.OperationName == "IntrospectionQuery";
        }
    }
}
