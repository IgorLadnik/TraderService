using GraphQL.Types;

namespace TraderModelLib.Queries
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery()
        {
            Field<TraderQuery>("traderQuery", resolve: context => new { });
            Field<TraderQuery>("activeTradersQuery", resolve: context => new { });
        }
    }
}

