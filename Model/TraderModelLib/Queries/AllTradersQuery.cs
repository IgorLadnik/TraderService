﻿using System.Linq;
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
                    var sortBy = context.GetArgument<string>("sortBy");

                    var traders = await repo.FetchAsync(dbContext => dbContext.Traders.Where(t => t.IsDeleted == isDeleted).ToList());

                    if (!string.IsNullOrEmpty(sortBy))
                    {
                        switch (sortBy.ToLower())
                        {
                            case "birthdate":
                            case "age":
                                traders = traders.OrderBy(t => t.Birthdate).ToList();
                                break;

                            case "!birthdate":
                            case "!age":
                                traders = traders.OrderByDescending(t => t.Birthdate).ToList();
                                break;

                            case "email":
                                traders = traders.OrderBy(t => t.Email).ToList();
                                break;

                            case "!email":
                                traders = traders.OrderByDescending(t => t.Email).ToList();
                                break;
                        }
                    }

                    context.SetCache<GqlCache>("traderIds", traders.Select(t => t.Id).ToList());

                    // Pagination
                    var pageSize = context.GetArgument<int>("pageSize");
                    if (pageSize == 0)
                        return traders;

                    var currentPage = context.GetArgument<int>("currentPage");
                    return traders.Skip(currentPage * pageSize).Take(pageSize).ToList();
                });
        }
    }
}
