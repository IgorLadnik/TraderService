using GraphQlHelperLib;
using RepoInterfaceLib;
using TraderModelLib.Data;
using TraderModelLib.Models;

namespace TraderModelLib.Type
{
    public class TraderToCurrencyType : ObjectGraphTypeCached<TraderToCurrency>
    {
        public TraderToCurrencyType(IRepo<TraderDbContext> repo)
        {
            Field(r => r.Id);
            Field(r => r.TraderId);
            Field(r => r.CurrencyId);
        }
    }
}
