using System;
using System.Threading.Tasks;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Storage;
using RepoInterfaceLib;

namespace RepoLib
{
    public class Repo<T> : IRepo<T> where T : DbContext, new()
    {
        private IDbContextTransaction _transaction = null;
        private T _dbContext = null;

        public Task<R> FetchAsync<R>(Func<T, R> func) =>
            Task.Run(() =>
            {
                using var dbContext = new T();
                return func(dbContext);
            });

        public Task BeginTransactionAsync() =>
            Task.Run(() =>
            {
                _dbContext = new T();
                _transaction = _dbContext.Database.BeginTransaction();
            });

        public Task<RepoResponse> CommitAsync() =>
            Task.Run(() =>
            {
            if (_transaction == null)
                return new() { OpStatus = RepoOperationStatus.Failure, Message = "No transactio open." };

            RepoResponse repoResponse = new() { OpStatus = RepoOperationStatus.Success, Message = string.Empty };
            try
            {
                _transaction.Commit();
            }
            catch (Exception e)
            {
                repoResponse = ErrorHandler(e);
            }
            finally
            {
                DisposeObjects();
            }

            return repoResponse;
        });

        public Task<RepoResponse> SaveAsync(Action<T> action) =>
            Task.Run(() =>
            {
                if (_transaction == null)
                    return new() { OpStatus = RepoOperationStatus.Failure, Message = "No transactio open." };

                RepoResponse repoResponse = new() { OpStatus = RepoOperationStatus.Success, Message = string.Empty };
                try
                {
                    action(_dbContext);
                    _dbContext.SaveChanges();
                }
                catch (Exception e)
                {
                    repoResponse = ErrorHandler(e);
                }

                return repoResponse;
            });

        private RepoResponse ErrorHandler(Exception e) 
        {
            DisposeObjects();
            return new() { OpStatus = RepoOperationStatus.Failure, Message = e.Message };
        }

        private void DisposeObjects() 
        {
            _transaction?.Dispose();
            _dbContext?.Dispose();

            _transaction = null;
            _dbContext = null;
        }
    }
}
