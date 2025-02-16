using System.IO;
using System.Text.Json;

namespace GOILauncher.Helpers
{
    internal class StorageHelper
    {
        public static void SaveJson(string path, string name, object obj,bool overwrite)
        {
            using StreamWriter sw = new(Path.Combine(path, name), !overwrite);
            sw.WriteLine(JsonSerializer.Serialize(obj));
            sw.Close();
        }
        public static T LoadJson<T>(string path, string name)
        {
            return JsonSerializer.Deserialize<T>(File.ReadAllText(Path.Combine(path, name)));
        }
    }
}
