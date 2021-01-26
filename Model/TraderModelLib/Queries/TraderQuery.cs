using System.Linq;
using GraphQL.Types;
using GraphQlHelperLib;
using TraderModelLib.Data;
using TraderModelLib.Type;
using RepoInterfaceLib;

namespace TraderModelLib.Queries
{
    public class TraderQuery : ObjectGraphType
    {
        public TraderQuery(IRepo<TraderDbContext> repo)
        {
            FieldAsync<ListGraphType<TraderType>>("traders",
                arguments: new QueryArguments(new QueryArgument<BooleanGraphType> { Name = "isDeleted" }), 
                resolve: async context =>
                {
                    var isDeleted = (bool)context.Arguments["isDeleted"];
                    var traders = await repo.FetchAsync(dbContext => dbContext.Traders.Where(t => t.IsDeleted == isDeleted).ToList());
                    context.SetCache<GqlCache>("traderIds", traders.Select(t => t.Id).ToList());
                    return traders;
                });
        }
    }
}

/*
query Traders {
  traderQuery {
    traders(isDeleted: false) {
      id
      firstName
      lastName
      birthdate
      email
      password
      isDeleted
      cryptocurrencies {
        id
        currency
        symbol
      }
    }
  }
}
*/