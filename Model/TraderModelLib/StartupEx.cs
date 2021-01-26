using System;
using Microsoft.Extensions.DependencyInjection;
using RepoInterfaceLib;
using RepoLib;
using TraderModelLib.Data;
using TraderModelLib.Type;
using TraderModelLib.Queries;
using TraderModelLib.Mutations;

namespace TraderModelLib
{
    public static class StartupEx
    { 
        public static void AddModelServices(this IServiceCollection services, string connectionString)
        {
            TraderDbContext.ConnectionString = connectionString;
            
            // DbContext
            services.AddSingleton<IRepo<TraderDbContext>, Repo<TraderDbContext>>();

            // Types
            services.AddTransient<CryptocurrencyType>();
            services.AddTransient<TraderToCurrencyType>();
            services.AddTransient<TraderType>();

            // Input types
            services.AddTransient<CryptocurrencyInputType>();
            services.AddTransient<TraderInputType>();

            // Output types
            services.AddTransient<TraderOutputType>();

            // Queries
            services.AddTransient<RootQuery>();
            services.AddTransient<TraderQuery>();
            services.AddTransient<ActiveTradersQuery>();

            // Mutations
            services.AddTransient<TraderMutation>();
            services.AddTransient<RootMutation>();
        } 
    }
}
