using GraphQL.Types;

namespace TraderModelLib.Queries
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery()
        {
            Field<TradersQuery>("tradersQuery", resolve: context => new { });
            Field<TraderByUniquePropertyQuery>("traderByPropertyQuery", resolve: context => new { });
        }
    }
}

