using GraphQL.Types;

namespace TraderModelLib.Mutations
{
    public class RootMutation : ObjectGraphType
    {
        public RootMutation()
        {
            Field<TradersMutation>("tradersMutation", resolve: contect => new { });
        }
    }
}

