using System.Collections.Generic;
using System.Threading.Tasks;

namespace Framework.Infrastructure.Services
{
    public interface IFileHelper
    {
        Task<bool> ExistsAsync(string filename); 

        Task WriteTextAsync(string filename, string text, bool encrypt = false); 

        Task<string> ReadTextAsync(string filename, bool encrypt = false); 

		Task<IEnumerable<string>> GetFilesAsync(); 

        Task DeleteAsync(string filename);

        string GetFilePath(string filename);
   	}
}
