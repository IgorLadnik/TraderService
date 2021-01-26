﻿using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Configuration;
using GraphQL;
using GraphQL.Types;
using GraphQlHelperLib;
//using AuthRolesLib;

namespace TraderService.Controllers
{
    [Route("[controller]")]
    [ApiController]
    public class GqlController : GqlControllerBase
    {
        public GqlController(ISchema schema, IDocumentExecuter documentExecuter, IConfiguration configuration)
            : base(schema, documentExecuter, configuration)
        {
        }

        [HttpPost]
        //[Route("auth")]
        //[AuthorizeRoles(UserAuthType.SuperUser)]
        public async Task<IActionResult> PostAsyncAuth([FromBody] GraphqlQuery query) =>
            await ProcessQuery(query/*, UserAuthRole.SuperUser, UserAuthRole.Admin*/);
    }
}