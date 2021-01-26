using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using GraphQL.Types;
using GraphQlHelperLib;
using RepoInterfaceLib;
using TraderModelLib.Data;
using TraderModelLib.Models;

namespace TraderModelLib.Type
{
    public class CryptocurrencyType : ObjectGraphTypeCached<Cryptocurrency>
    {
        public CryptocurrencyType(IRepo<TraderDbContext> repo)
        {
            Field(c => c.Id);
            Field(c => c.Currency);
            Field(c => c.Symbol);
        }
    }
}
