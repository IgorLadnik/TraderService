using System;
using System.Security.Claims;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using Newtonsoft.Json.Linq;
using GraphQL;
using GraphQL.NewtonsoftJson;
using GraphQL.Types;
//using AuthRolesLib;

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

        public async Task<ExecutionResult> Process(GraphqlQuery query, ClaimsPrincipal user/*, params UserAuthRole[] roles*/)
        {
            if (query == null)
                throw new ArgumentNullException(nameof(query));

            //TEST
            if (!query.IsIntrospection)
            {
            }

            var executionOptions = new ExecutionOptions
            {
                Query = query.Query,
                Inputs = query.Variables.ToInputs(),
                //ValidationRules = validationRules
            };

            return await SetParamsAndExecute(executionOptions, user/*, roles*/);
        }

        public async Task<ExecutionResult> Process(string query, ClaimsPrincipal user/*, params UserAuthRole[] roles*/)
        {
            if (string.IsNullOrEmpty(query))
                throw new ArgumentNullException(nameof(query));

            var executionOptions = new ExecutionOptions
            {
                Query = query,
            };

            return await SetParamsAndExecute(executionOptions, user/*, roles*/);
        }

        private async Task<ExecutionResult> SetParamsAndExecute(ExecutionOptions executionOptions, ClaimsPrincipal user/*, params UserAuthRole[] roles*/)
        {
            executionOptions.Schema = _schema;
            executionOptions.SetIsAuthJwt(_isAuthJwt);
            executionOptions.SetUser(user);
            return Validate(executionOptions/*, roles*/) ?? await _documentExecuter.ExecuteAsync(executionOptions);
        }

        private static ExecutionResult Validate(ExecutionOptions executionOptions/*, params UserAuthRole[] roles*/) 
        {
            //try
            //{
            //    executionOptions.ValidateRole(roles); // Auth. filter
            //}
            //catch (Exception e)
            //{
            //    ExecutionResult er = new() { Errors = new() };
            //    er.Errors.Add(new ExecutionError(e.Message, e));
            //    return er;
            //}

            return null;
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
