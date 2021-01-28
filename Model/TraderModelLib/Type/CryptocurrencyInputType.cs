using GraphQL.Types;

namespace TraderModelLib.Type
{
    public class CryptocurrencyInputType : InputObjectGraphType
    {
        public CryptocurrencyInputType()
        {
            Field<IntGraphType>("Id");
        }
    }
}
