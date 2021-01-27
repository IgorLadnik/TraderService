using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQlHelperLib;
using RepoInterfaceLib;
using TraderModelLib.Data;
using TraderModelLib.Models;

namespace TraderModelLib.Type
{
    public class TraderType : ObjectGraphTypeCached<Trader>
    {
        public TraderType(IRepo<TraderDbContext> repo)
        {
            Field(p => p.Id);
            Field(p => p.FirstName);
            Field(p => p.LastName);
            Field(p => p.Birthdate);
            Field(p => p.Email);
            Field(p => p.Password);
            Field(p => p.IsDeleted);

            FieldAsync<ListGraphType<CryptocurrencyType>>("cryptocurrencies", resolve: async context =>
            {
                Console.WriteLine("before 1");

                await CacheDataFromRepo(async () =>
                {
                    if (context.DoesCacheExist("cryptocurrencies"))
                        return;

                    Console.WriteLine("** fetch 1");

                    var traderIds = context.GetCache<IList<int>>("traderIds");
                    var t2cs = await repo.FetchAsync(dbContext => dbContext.T2Cs.Where(a => traderIds.Contains(a.TraderId)).ToList());                 
                    var currencyIds = t2cs.Select(t => t.CurrencyId).ToList();
                    var cryptocurrencies = await repo.FetchAsync(dbContext => dbContext.Cryptocurrencies.Where(c => currencyIds.Contains(c.Id)).ToList());
                    context.SetCache<GqlCache>("t2cs", t2cs);
                    context.SetCache<GqlCache>("cryptocurrencies", cryptocurrencies);
                });

                Console.WriteLine("after 1");

                var t2cs1 = context.GetCache<IList<TraderToCurrency>>("t2cs");
                var cryptocurrencies1 = context.GetCache<IList<Cryptocurrency>>("cryptocurrencies");
                var currencyIds = t2cs1.Where(tc => tc.TraderId == context.Source.Id).Select(a => a.CurrencyId).ToList();
                return cryptocurrencies1.Where(c => currencyIds.Contains(c.Id)).ToList();
            });
        }
    }
}
