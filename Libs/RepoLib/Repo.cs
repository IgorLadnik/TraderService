using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using RepoInterfaceLib;

namespace RepoLib
{
    public class Repo<T> : IRepo<T> where T : DbContext, new()
    {
        public Task<R> FetchAsync<R>(Func<T, R> func) =>
            Task.Run(() =>
            {
                using var dbContext = new T();
                return func(dbContext);
            });
        
        public Task<RepoResponse> SaveAsync(Action<T> action) =>
            Task.Run(() =>
            {
                RepoResponse repoResponse = new() { Status = "Success", Message = string.Empty };
                using var dbContext = new T();
                using var transaction = dbContext.Database.BeginTransaction();
                try
                {
                    action(dbContext);
                    dbContext.SaveChanges();
                    transaction.Commit();
                }
                catch (Exception e) 
                {
                    repoResponse.Status = "Failure";
                    repoResponse.Message = e.Message;
                }

                return repoResponse;
            });
    }
}
