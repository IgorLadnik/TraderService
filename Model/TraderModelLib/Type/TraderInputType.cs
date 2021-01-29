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
            Field<StringGraphType>("Avatar");
            Field<StringGraphType>("Email");
            Field<StringGraphType>("Password");
            Field<BooleanGraphType>("IsDeleted");
            Field<ListGraphType<CryptocurrencyInputType>>("Cryptocurrencies");
        }
    }
}
