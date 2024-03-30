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

        public async static Task<string> GetUIDFromBVID(string bvid)
        {
            using(HttpClient  client = new HttpClient())
            {
                var response = await client.GetAsync($"https://api.bilibili.com/x/web-interface/view?bvid={bvid}");
                string content = await response.Content.ReadAsStringAsync();
                var bilibiliResponse = JsonConvert.DeserializeObject<BilibiliResponse>(content);
                if(bilibiliResponse.code == 0)
                {
                    var videoResponse = JsonConvert.DeserializeObject<VideoResponse>(content);
                    return videoResponse.data.owner.mid.ToString();
                }
                else
                {
                    return string.Empty;
                }
            }
        }

        public async static Task<string> GetUserNameFromUID(string uid)
        {
            using (HttpClient client = new HttpClient())
            {
                var response = await client.GetAsync($"https://api.live.bilibili.com/live_user/v1/Master/info?uid={uid}");
                string content = await response.Content.ReadAsStringAsync();
                var bilibiliResponse = JsonConvert.DeserializeObject<BilibiliResponse>(content);
                if (bilibiliResponse.code == 0)
                {
                    var liveUserResponse = JsonConvert.DeserializeObject<LiveUserResponse>(content);
                    return liveUserResponse.data.info.uname;
                }
                else
                {
                    return string.Empty;
                }
            }
        }
    }
    public class BilibiliResponse
    {
        public int code;
    }
    public class VideoResponse
    {
        public VideoDataResponse data = new VideoDataResponse();
    }
    public class VideoDataResponse
    {
        public VideoOwnerResponse owner = new VideoOwnerResponse();
    }
    public class VideoOwnerResponse
    {
        public int mid;
    }
    public class LiveUserResponse
    {
        public LiveUserDataResponse data = new LiveUserDataResponse();
    }
    public class LiveUserDataResponse
    {
        public LiveUserInfoResponse info = new LiveUserInfoResponse();
    }
    public class LiveUserInfoResponse
    {
        public string uname = string.Empty;
    }

}
