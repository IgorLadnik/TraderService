using System;
using System.Threading.Tasks;
using GraphQL.Types;
using AsyncLockLib;

namespace GraphQlHelperLib
{
    public class ObjectGraphTypeCached<T> : ObjectGraphType<T>
    {
        private readonly AsyncLock _lock = new();
        private string _typeName;


        public ObjectGraphTypeCached()
        {
            _typeName = typeof(T).FullName;
        }

        protected Task CacheDataFromRepo(Func<bool> shouldFetchFromRepo, Func<Task> fetchFromRepo) =>
            Task.Run(async () =>
            {
                using (await _lock.LockAsync())
                    if (shouldFetchFromRepo())
                        await fetchFromRepo();
            });
    }
}
