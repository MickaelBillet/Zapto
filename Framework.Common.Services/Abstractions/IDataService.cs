using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface IDataService
    {
        Task SaveDataAsync<T>(string folderPath, T data);

        Task<T> LoadDataAsync<T>(string folderPath) where T : class;
	}
}
