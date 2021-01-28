using GraphQlHelperLib;
using TraderModelLib.Models;

namespace TraderModelLib.Type
{
    public class TraderToCurrencyType : ObjectGraphTypeCached<TraderToCurrency>
    {
        public TraderToCurrencyType()
        {
            Field(r => r.Id);
            Field(r => r.TraderId);
            Field(r => r.CurrencyId);
        }
    }
}
