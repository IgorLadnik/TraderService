using System;
using System.Collections.Generic;
using System.Linq;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using GraphQL;
using GraphQL.Types;
using RepoInterfaceLib;
using TraderModelLib.Data;
using TraderModelLib.Models;
using TraderModelLib.Type;

namespace TraderModelLib.Mutations
{
    public class TradersMutation : ObjectGraphType
    {
        public TradersMutation(IRepo<TraderDbContext> repo, ILogger<ControllerBase> logger)
        {
            FieldAsync<TraderOutputType>("createTraders",
                arguments: new QueryArguments(new QueryArgument<ListGraphType<TraderInputType>> { Name = "tradersInput" }),
                resolve: async context =>
                {
                    logger.LogTrace("TradersMutation called");

                    List<Trader> traders = new();
                    Dictionary<string, List<int>> dctTraderEmailToCurrencyId = new(); 
                    
                    foreach (var traderInput in context.GetArgument<Dictionary<string, object>[]>("tradersInput"))
                    {
                        Trader trader = new();
                        List<int> t2cInputs = new();
                        TraderToCurrency t2c = new();

                        object v;
                        foreach (var prop in traderInput)
                            switch (prop.Key)
                            {
                                case "cryptocurrencies":
                                    foreach (Dictionary<string, object> dctProp in (IList<object>)prop.Value)
                                        foreach (var key in dctProp.Keys)
                                        {
                                            v = dctProp[key];
                                            switch (key)
                                            {
                                                case "id": t2cInputs.Add((int)v); break;
                                            }
                                        }
                                    break;

                                default:
                                    v = prop.Value;
                                    switch (prop.Key)
                                    {
                                        case "firstName": trader.FirstName = (string)v; break;
                                        case "lastName": trader.LastName = (string)v; break;
                                        case "birthdate": trader.Birthdate = (DateTime)v; break;
                                        case "avatar": trader.Avatar = ((string)v)?.ToLower(); break;
                                        case "email": trader.Email = ((string)v)?.ToLower(); break;
                                        case "password": trader.Password = (string)v; break;
                                        case "isDeleted": trader.IsDeleted = (bool)v; break;
                                    }
                                    break;
                            }

                        dctTraderEmailToCurrencyId[trader.Email] = t2cInputs;

                        traders.Add(trader);
                    }

                    // Check if creating traders exist - to update them
                    List<Trader> tradersToUpdate = new();
                    List<Trader> tradersToInsert = new();
                    List<TraderToCurrency> t2csToRemove = new();
                    List<TraderToCurrency> t2csToInsert = new();

                    List<int> traderIds = new();

                    RepoResponse mutationResponse = new() { OpStatus = RepoOperationStatus.Success };
                    var emails = traders?.Select(t => t.Email)?.ToList();
                    if (emails?.Count > 0)
                    {
                        var existingTraders = await repo.FetchAsync(dbContext => dbContext.Traders.Where(t => emails.Contains(t.Email)).ToList());
                        foreach (var trader in traders)
                        {
                            var existingTrader = existingTraders?.Where(et => et.Email == trader.Email).FirstOrDefault();
                            if (existingTrader != null)
                            {
                                trader.Id = existingTrader.Id;

                                tradersToUpdate.Add(trader);
                                traderIds.Add(trader.Id);
                            }
                            else
                            {
                                trader.Id = TraderDbContext.CurrentId;
                                tradersToInsert.Add(trader);
                            }

                            foreach (var email in dctTraderEmailToCurrencyId.Keys)
                                if (email == trader.Email)
                                    foreach (var currencyId in dctTraderEmailToCurrencyId[email])
                                        t2csToInsert.Add(new TraderToCurrency { Id = TraderDbContext.CurrentId, TraderId = trader.Id, CurrencyId = currencyId });
                        }

                        t2csToRemove = await repo.FetchAsync(dbContext => dbContext.T2Cs?.Where(t => traderIds.Contains(t.TraderId)).ToList());
                    }

                    // Should be unique TraderId - CurrencyId pairs to insert
                    Dictionary<string, int> dctUnique = new();
                    foreach (var t in t2csToInsert) 
                        dctUnique[$"{t.TraderId}{t.CurrencyId}"] = t.Id;
                    var t2csUniqueToInsert = t2csToInsert.Where(t => dctUnique.Values.Contains(t.Id)).ToList();

                    // Save changes with a transaction 
                    var result = await repo.SaveAsync(dbContext =>
                    {
                        dbContext.T2Cs?.RemoveRange(t2csToRemove);
                        dbContext.Traders?.UpdateRange(tradersToUpdate);
                        dbContext.Traders?.AddRange(tradersToInsert);
                        dbContext.T2Cs?.AddRange(t2csUniqueToInsert);
                    });

                    if (result.IsOK)
                        logger.LogTrace("TradersMutation: changes saved");
                    else
                        logger.LogError($"TradersMutation: ERROR. {result.Message}");

                    return result;
                });
        }
    }
}


