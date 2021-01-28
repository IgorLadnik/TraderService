using GraphQlHelperLib;
using TraderModelLib.Models;

namespace TraderModelLib.Type
{
    public class CryptocurrencyType : ObjectGraphTypeCached<Cryptocurrency>
    {
        public CryptocurrencyType()
        {
            Field(c => c.Id);
            Field(c => c.Currency);
            Field(c => c.Symbol);
        }
    }
}
