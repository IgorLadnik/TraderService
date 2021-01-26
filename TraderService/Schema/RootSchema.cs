using System;
using Microsoft.Extensions.DependencyInjection;
using TraderModelLib.Queries;
using TraderModelLib.Mutations;

namespace TraderService.Schema
{
    public class RootSchema : GraphQL.Types.Schema
    {
        public RootSchema(IServiceProvider serviceProvider) : base(serviceProvider)
        {
            Query = serviceProvider.GetRequiredService<RootQuery>();
            Mutation = serviceProvider.GetRequiredService<RootMutation>();
        }
    }
}

