using System;
using System.Threading.Tasks;

namespace RepoInterfaceLib
{
    public interface IRepo<TStorateContext>
    {
        Task<R> FetchAsync<R>(Func<TStorateContext, R> func);

        Task BeginTransactionAsync();

        Task<RepoResponse> CommitAsync();

        Task<RepoResponse> SaveAsync(Action<TStorateContext> action);
    }
}
