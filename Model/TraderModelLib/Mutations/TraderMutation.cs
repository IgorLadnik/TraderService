﻿using System;
using System.Collections.Generic;
using GraphQL;
using GraphQL.Types;
using RepoInterfaceLib;
using TraderModelLib.Data;
using TraderModelLib.Models;
using TraderModelLib.Type;

namespace TraderModelLib.Mutations
{
    public class TraderMutation : ObjectGraphType
    {
        public TraderMutation(IRepo<TraderDbContext> repo)
        {
            FieldAsync<TraderOutputType>("createTraders",
                arguments: new QueryArguments(new QueryArgument<ListGraphType<TraderInputType>> { Name = "tradersInput" }),
                resolve: async context =>
                {
                    List<Trader> traders = new();
                    //List<Cryptocurrency> cryptocurrencies = new();
                    List<TraderToCurrency> t2cs = new();

                    foreach (var traderInput in context.GetArgument<Dictionary<string, object>[]>("tradersInput"))
                    {
                        Trader trader = new();
                        Cryptocurrency cryptocurrency = new();
                        TraderToCurrency t2c = new();

                        object v;
                        foreach (var item in traderInput)
                        {
                            var prop = (KeyValuePair<string, object>)item;
                            switch (prop.Key)
                            {
                                case "cryptocurrencies":
                                    foreach (Dictionary<string, object> dctProp in (IList<object>)prop.Value)
                                    {
                                        foreach (var key in dctProp.Keys)
                                        {
                                            v = dctProp[key];
                                            switch (key)
                                            {
                                                case "id": cryptocurrency.Id = (int)v; break;
                                                //case "currency": cryptocurrency.Currency = (string)v; break;
                                                //case "symbol": cryptocurrency.Symbol = (string)v; break;
                                            }
                                        }
                                    }
                                    break;

                                default:
                                    v = prop.Value;
                                    switch (prop.Key)
                                    {
                                        case "id": trader.Id = (int)v; break;
                                        case "firstName": trader.FirstName = (string)v; break;
                                        case "lastName": trader.LastName = (string)v; break;
                                        case "birthdate": trader.Birthdate = (DateTime)v; break;
                                        case "email": trader.Email = (string)v; break;
                                        case "password": trader.Password = (string)v; break;
                                        case "isDeleted": trader.IsDeleted = (bool)v; break;
                                    }
                                    break;
                            }
                        }

                        t2c.TraderId = trader.Id;
                        t2c.CurrencyId = cryptocurrency.Id;

                        traders.Add(trader);
                        //cryptocurrencies.Add(cryptocurrency);
                        t2cs.Add(t2c);
                    }

                    var mutationResponse = await repo.SaveAsync(dbContext =>
                    {
                        dbContext.Traders.AddRange(traders);
                        //dbContext.Cryptocurrencies.AddRange(cryptocurrencies);
                        dbContext.T2Cs.AddRange(t2cs);
                    });

                    return mutationResponse;
                });
        }
    }
}

/*
mutation TraderMutation {
  traderMutation {
    createTraders(
      tradersInput: [{
        id: 10
        firstName: "Amit"
        lastName: "Mukerjee"
        birthdate: "1960-01-01"
        email: "amitm@trader.com"
        password: "aaa"         
        cryptocurrencies: [{
       	  id: 1
        }]
   	  }]
    ) {
      status
      message
    }
  }
}
*/
