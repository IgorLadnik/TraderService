using System.Linq;
using System.Linq.Dynamic.Core;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GraphQL;
using GraphQL.Types;
using GraphQlHelperLib;
using TraderModelLib.Data;
using TraderModelLib.Type;
using RepoInterfaceLib;

namespace TraderModelLib.Queries
{
    public class TradersQuery : ObjectGraphType
    {
        public TradersQuery(IRepo<TraderDbContext> repo, ILogger<ControllerBase> logger)
        {
            FieldAsync<ListGraphType<TraderType>>("traders",
                arguments: new QueryArguments(
                        new QueryArgument<BooleanGraphType> { Name = "isDeleted" },
                        //new QueryArgument<FilterInputType> { Name = "filter" }, // for future use
                        new QueryArgument<StringGraphType> { Name = "sortBy" },
                        new QueryArgument<IntGraphType> { Name = "pageSize" },
                        new QueryArgument<IntGraphType> { Name = "currentPage" }
                    ), 
                resolve: async context =>
                {
                    logger.LogTrace("TradersQuery called");

                    // for future use
                    //var filter = context.GetArgument("filter");
                    //if (filter != null) 
                    //{
                    //    var propertyName = filter["property"];
                    //    var rule = filter["rule"];
                    //}

                    var isDeleted = context.GetArgument<bool>("isDeleted");
                    var traders = await repo.FetchAsync(dbContext => dbContext.Traders.Where(t => t.IsDeleted == isDeleted).ToList());

                    // Sort by a property
                    var sortBy = context.GetArgument<string>("sortBy");
                    traders = traders.Sort(sortBy);

                    context.SetCache<GqlCache>("traderIds", traders.Select(t => t.Id).ToList());

                    // Pagination
                    var pageSize = context.GetArgument<int>("pageSize");
                    var currentPage = context.GetArgument<int>("currentPage");
                    return traders.Page(pageSize, currentPage);
                });
        }
    }
}
