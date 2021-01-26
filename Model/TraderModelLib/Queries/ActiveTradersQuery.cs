using System.Linq;
using GraphQL.Types;
using GraphQlHelperLib;
using TraderModelLib.Data;
using TraderModelLib.Type;
using RepoInterfaceLib;

namespace TraderModelLib.Queries
{
    public class ActiveTradersQuery : ObjectGraphType
    {
        public ActiveTradersQuery(IRepo<TraderDbContext> repo)
        {
            //FieldAsync<ListGraphType<TraderType>>("traders",
            //    arguments: new QueryArguments(new QueryArgument<BooleanGraphType> { IsDeleted = "id" }), 
            //    resolve: async context =>
            //    {
            //        var traders = await repo.FetchAsync(dbContext => dbContext.Traders.ToList());
            //        context.SetCache<GqlCache>("traderIds", traders.Select(t => t.Id).ToList());
            //        return traders;
            //    });
        }
    }
}

/*
query Traders {
  traderQuery {
    traders {
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