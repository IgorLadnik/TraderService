using GraphQL.Types;

namespace TraderModelLib.Queries
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery()
        {
            Field<AllTradersQuery>("allTradersQuery", resolve: context => new { });
        }
    }
}

