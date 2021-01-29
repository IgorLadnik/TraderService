﻿using System;
using System.Threading.Tasks;

namespace RepoInterfaceLib
{
    public interface IRepo<TStorateContext>
    {
        Task<R> FetchAsync<R>(Func<TStorateContext, R> func);

        Task BeginTransactionAsync();

        Task<R> FetchInTransactionAsync<R>(Func<TStorateContext, R> func);

        Task<RepoResponse> SaveAsync(Action<TStorateContext> action);

        Task<RepoResponse> CommitAsync();
    }
}
