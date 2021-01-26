using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using RepoInterfaceLib;

namespace TraderModelLib.Type
{
    public class TraderOutputType : ObjectGraphType<RepoResponse>
    {
        public TraderOutputType()
        {
            Field(p => p.Status);
            Field(p => p.Message);
        }
    }
}
