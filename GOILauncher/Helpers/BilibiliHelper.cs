using Avalonia.Automation;
using LC.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.Helpers
{
    internal class BilibiliHelper
    {
        public async static Task<BlibiliResult> GetResultFromBVID(string bvid)
        {
            using HttpClient client = new();
            var response = await client.GetAsync($"https://api.bilibili.com/x/web-interface/view?bvid={bvid}");
            string content = await response.Content.ReadAsStringAsync();
            var bilibiliResponse = JsonConvert.DeserializeObject<BilibiliResponse>(content);
            if (bilibiliResponse.code == 0)
            {
                return new BlibiliResult()
                {
                    UID = bilibiliResponse.data.owner.mid,
                    Name = bilibiliResponse.data.owner.name
                };
            }
            else
            {
                return new BlibiliResult();
            }
        }
        public class BilibiliResponse
        {
            public int code;
            public Data data = new();
        }
        public class Data
        {
            public Owner owner = new();
        }
        public class Owner
        {
            public string? mid;
            public string? name;
        }
        public class BlibiliResult
        {
            public string UID { get; set; }
            public string? Name { get; set; }
        }
    }
}