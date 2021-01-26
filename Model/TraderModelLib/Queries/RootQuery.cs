using GraphQL.Types;

namespace TraderModelLib.Queries
{
    public class RootQuery : ObjectGraphType
    {
        public RootQuery()
        {
            Field<TraderQuery>("traderQuery", resolve: context => new { });
            //Field<PersonByIdQuery>("personByIdQuery", resolve: context => new { });
            //Field<OrganizationQuery>("organizationQuery", resolve: context => new { });
        }
    }
}

