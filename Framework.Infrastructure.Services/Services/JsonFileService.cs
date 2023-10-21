using System;
using System.IO;
using System.Text.Json;
using System.Threading.Tasks;

#nullable disable

namespace Framework.Infrastructure.Services
{
    internal class JsonFileService : IJsonFileService
    {
        #region Properties

        #endregion

        #region Constructor

        public JsonFileService()
        {
        }

        #endregion

        public async Task WriteFile<T>(T item) where T : class
        {
            string jsonfile = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\NOVACOR\Novatools\" + typeof(T).Name + @".json";

            if (item != null)
            {
                using (StreamWriter file = File.CreateText(jsonfile))
                {
                    await JsonSerializer.SerializeAsync<T>(file.BaseStream, item);
                }
            }
            else
            {
                File.Delete(jsonfile);
            }
        }

        public async Task<T> ReadFile<T>() where T : class
        {
            T item = null;

            string jsonfile = Environment.GetFolderPath(Environment.SpecialFolder.CommonApplicationData) + @"\NOVACOR\Novatools\" + typeof(T).Name + @".json";

            if (File.Exists(jsonfile))
            {
                using (StreamReader file = File.OpenText(jsonfile))
                {
                    item = (T)(await JsonSerializer.DeserializeAsync(file.BaseStream, typeof(T)));
                }
            }

            return item;
        }
    }
}
