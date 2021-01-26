using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;

namespace TraderModelLib.Type
{
    public class CryptocurrencyInputType : InputObjectGraphType
    {
        public CryptocurrencyInputType()
        {
            Field<IntGraphType>("Id");
            Field<StringGraphType>("Currency");
            Field<StringGraphType>("Symbol");
        }
    }
}
