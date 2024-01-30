using LC.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace GOI地图管理器.Helpers
{
    internal class StorageHelper
    {
        public static void SaveJSON(string path, string name, object obj,bool overwrite)
        {
            using (StreamWriter sw = new StreamWriter(Path.Combine(path, name), !overwrite))
            {
                sw.WriteLine(JsonConvert.SerializeObject(obj));
                sw.Close();
                sw.Dispose();
            }
        }
        public static T LoadJSON<T>(string path, string name)
        {
            return (T)(JsonConvert.DeserializeObject<T>(File.ReadAllText(Path.Combine(path, name))));
        }
    }
}
