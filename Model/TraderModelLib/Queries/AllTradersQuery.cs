using System.Linq;
using System.Linq.Dynamic.Core;
using GraphQL.Types;
using GraphQlHelperLib;
using TraderModelLib.Data;
using TraderModelLib.Type;
using RepoInterfaceLib;

namespace TraderModelLib.Queries
{
    public class AllTradersQuery : ObjectGraphType
    {
        public AllTradersQuery(IRepo<TraderDbContext> repo)
        {
            FieldAsync<ListGraphType<TraderType>>("traders",
                arguments: new QueryArguments(
                        new QueryArgument<BooleanGraphType> { Name = "isDeleted" },
                        new QueryArgument<StringGraphType> { Name = "sortBy" },
                        new QueryArgument<IntGraphType> { Name = "pageSize" },
                        new QueryArgument<IntGraphType> { Name = "currentPage" }
                    ), 
                resolve: async context =>
                {
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
