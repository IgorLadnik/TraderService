using GraphQL.Types;

namespace TraderModelLib.Type
{
    public class FilterInputType : InputObjectGraphType
    {
        public FilterInputType()
        {
            Field<StringGraphType>("Property");
            Field<StringGraphType>("Rule");
        }
    }
}
