using LC.Newtonsoft.Json;
using System.Net.Http;
using System.Threading.Tasks;

namespace GOILauncher.Helpers
{
    internal class BilibiliHelper
    {
        public static async Task<BilibiliResult> GetResultFromBVID(string bvid)
        {
            using HttpClient client = new();
            var response = await client.GetAsync($"https://api.bilibili.com/x/web-interface/view?bvid={bvid}");
            var content = await response.Content.ReadAsStringAsync();
            var bilibiliResponse = JsonConvert.DeserializeObject<BilibiliResponse>(content);
            if (bilibiliResponse.code == 0)
            {
                return new BilibiliResult()
                {
                    UID = bilibiliResponse.data.owner.mid,
                    Name = bilibiliResponse.data.owner.name
                };
            }
            else
            {
                return new BilibiliResult() { UID = string.Empty };
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
        public class BilibiliResult
        {
            public required string? UID { get; set; }
            public string? Name { get; set; }
        }
    }
}