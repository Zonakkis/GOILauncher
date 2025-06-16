using GOILauncher.Infrastructures.LeanCloud;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace GOILauncher.Infrastructures.Interfaces
{
    public interface ILeanCloud
    {
        Task<int> Count<T>(LeanCloudQuery<T> leanCloudQuery);
        Task Create<T>(T obj);
        Task<List<T>> Find<T>(LeanCloudQuery<T> leanCloudQuery);
        Task<T> Get<T>(string objectId);
    }
}