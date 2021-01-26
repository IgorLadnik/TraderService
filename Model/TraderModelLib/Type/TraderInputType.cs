using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;

namespace TraderModelLib.Type
{
    public class TraderInputType :  InputObjectGraphType
    {
        public TraderInputType()
        {
            Field<StringGraphType>("FirstName");
            Field<StringGraphType>("LastName");
            Field<DateGraphType>("Birthdate");
            Field<StringGraphType>("Email");
            Field<StringGraphType>("Password");
            Field<BooleanGraphType>("IsDeleted");
            Field<ListGraphType<CryptocurrencyInputType>>("Cryptocurrencies");
        }
    }
}
