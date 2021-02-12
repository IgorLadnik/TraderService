using System.Linq;
using System.Linq.Dynamic.Core;
using System.Collections.Generic;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GraphQL;
using GraphQL.Types;
using GraphQlHelperLib;
using TraderModelLib.Data;
using TraderModelLib.Type;
using RepoInterfaceLib;
using TraderModelLib.Models;

namespace TraderModelLib.Queries
{
    public class TraderByUniquePropertyQuery : ObjectGraphType
    {
        public TraderByUniquePropertyQuery(IRepo<TraderDbContext> repo, ILogger<ControllerBase> logger)
        {
            FieldAsync<TraderType>("traderByUniqueProperty",
                arguments: new QueryArguments(
                        new QueryArgument<StringGraphType> { Name = "email" },
                        new QueryArgument<IntGraphType> { Name = "id" }
                    ),
                resolve: async context =>
                {
                    logger.LogTrace("TraderByUniquePropertyQuery called");

                    var email = context.GetArgument<string>("email");
                    Trader trader = null;
                    if (!string.IsNullOrEmpty(email))
                        trader = await repo.FetchAsync(dbContext => dbContext.Traders.Where(t => t.Email == email).FirstOrDefault());

                    if (trader == null) 
                    {
                        var id = context.GetArgument<int>("id");
                        trader = await repo.FetchAsync(dbContext => dbContext.Traders.Where(t => t.Id == id).FirstOrDefault());
                    }

                    if (trader != null)
                        context.SetCache<GqlCache>("traderIds", new List<int> { trader.Id });

                    return trader;
                });
        }
    }
}
