using Framework.Infrastructure.Services;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;

namespace AirZapto.Droid.Services
{
    class FileHelper : IFileHelper
    {
        public Task<bool> ExistsAsync(string filename)
        {
            string filepath = GetFilePath(filename);
            bool exists = File.Exists(filepath);

            return Task.FromResult(exists);
        }

        public async Task WriteTextAsync(string filename, string text, bool encrypt = false)
        {
            string filepath = GetFilePath(filename);

            using (StreamWriter writer = File.CreateText(filepath))
            {
                await writer.WriteAsync(text);
            }
        }

        public async Task<string> ReadTextAsync(string filename, bool encrypt = false)
        {
            string filepath = GetFilePath(filename);
            string data = null;

            using (StreamReader reader = File.OpenText(filepath))
            {
                data = await reader.ReadToEndAsync();
            }

            return data;
        }

        public Task<IEnumerable<string>> GetFilesAsync()
        {
            // Sort the filenames. 
            IEnumerable<string> filenames =
                from filepath in Directory.EnumerateFiles(FileHelper.GetApplicationDataFolder())
                select Path.GetFileName(filepath);

            return Task.FromResult(filenames);
        }

        public Task DeleteAsync(string filename)
        {
            File.Delete(GetFilePath(filename));

            return Task.FromResult(true);
        }

        public static string GetApplicationDataFolder()
        {
            return Environment.GetFolderPath(Environment.SpecialFolder.MyDocuments);
        }

        public string GetFilePath(string filename)
        {
            return Path.Combine(FileHelper.GetApplicationDataFolder(), filename);
        }
    }
} 
