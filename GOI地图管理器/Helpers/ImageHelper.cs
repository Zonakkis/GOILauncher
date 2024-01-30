using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Net.Http.Headers;
using System.Net.Http;
using System.Runtime.CompilerServices;
using System.Text;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;

namespace GOI地图管理器.Helpers
{
    public static class ImageHelper
    {
        public static async Task<Bitmap?> LoadFromWeb(string resourcePath)
        {
            Uri uri = new Uri(resourcePath);
            using var httpClient = new HttpClient();
            try
            {
                httpClient.DefaultRequestHeaders.UserAgent.Add(new ProductInfoHeaderValue("GOIMapManager", "0"));
                var data = await httpClient.GetByteArrayAsync(uri);
                return new Bitmap(new MemoryStream(data));
            }
            catch (HttpRequestException ex)
            {
                Console.WriteLine($"An error occurred while downloading image '{uri}' : {ex.Message}");
                return null;
            }
        }
    }
}
