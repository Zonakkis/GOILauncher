using LC.Newtonsoft.Json;
using System.IO;

namespace GOILauncher.Helpers
{
    internal class StorageHelper
    {
        public static void SaveJson(string path, string name, object obj,bool overwrite)
        {
            using StreamWriter sw = new(Path.Combine(path, name), !overwrite);
            sw.WriteLine(JsonConvert.SerializeObject(obj));
            sw.Close();
        }
        public static T LoadJson<T>(string path, string name)
        {
            return JsonConvert.DeserializeObject<T>(File.ReadAllText(Path.Combine(path, name)));
        }
    }
}
