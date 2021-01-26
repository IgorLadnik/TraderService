using GraphQL.Types;

namespace TraderModelLib.Mutations
{
    public class RootMutation : ObjectGraphType
    {
        public RootMutation()
        {
            Field<TraderMutation>("traderMutation", resolve: contect => new { });
        }
    }
}

