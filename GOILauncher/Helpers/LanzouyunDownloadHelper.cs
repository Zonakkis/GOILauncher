using Downloader;
using DynamicData;
using GOILauncher.Models;
using GOILauncher.ViewModels;
using LC.Newtonsoft.Json;
using LC.Newtonsoft.Json.Linq;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Net.Http.Json;
using System.Runtime.InteropServices.Marshalling;
using System.Security.Policy;
using System.Text;
using System.Text.RegularExpressions;
using System.Threading;
using System.Threading.Tasks;

namespace GOILauncher.Helpers
{
    internal class LanzouyunDownloadHelper
    {
        public static string prefix { get; set; } = string.Empty;
        public static HttpClient HttpClient => MainWindowViewModel.HttpClient;
        public static async Task Download(string url,string fileName,EventHandler<DownloadStartedEventArgs> downloadStartedEvent, EventHandler<Downloader.DownloadProgressChangedEventArgs> downloadProgressChangedEventArgs, EventHandler<AsyncCompletedEventArgs>? downloadCompletedEvent,CancellationToken cancellationToken = default)
        {
            var downloadOpt = new DownloadConfiguration()
            {
                ChunkCount = 16,
                ParallelDownload = true,
                MaxTryAgainOnFailover = int.MaxValue,
                Timeout = 60000,
                //MaximumMemoryBufferBytes = 1024 * 1024 * 50,
                RequestConfiguration =
                {
                    KeepAlive = true,
                    UserAgent="Mozilla/5.0 (Windows NT 10.0; Win64; x64; rv:92.0) Gecko/20100101 Firefox/92.0",
                    ProtocolVersion = HttpVersion.Version11,
                }
            };
            using DownloadService downloadService = new(downloadOpt);
            downloadService.DownloadStarted += downloadStartedEvent;
            downloadService.DownloadProgressChanged += downloadProgressChangedEventArgs;
            downloadService.DownloadFileCompleted += downloadCompletedEvent;
            await downloadService.DownloadFileTaskAsync(url, fileName, cancellationToken);
        }

        public static async Task<string> GetDirectURLAsync(string code)
        {
            try
            {
                string url = $"{prefix}/{code}";
                url = await GetDownloadPage(url);
                string sign = await GetSign(url);
                LanzouResponse lanzouResponse = await GetLanzouResponse(url , sign);
                return await GetRealURL($"{lanzouResponse.dom}/file/{lanzouResponse.url}");
            }
            catch (Exception ex)
            {
                NotificationHelper.ShowNotification("发生错误！", $"获取直链失败：{ex.Message}", FluentAvalonia.UI.Controls.InfoBarSeverity.Error);
                Trace.WriteLine($"获取直链失败：{ex.Message}");
                return await GetDirectURLAsync(code);
            }
        }
        public static async Task<string> GetDownloadPage(string url)
        {
            string content1 = await HttpClient.GetStringAsync(url);
            string regex = "\"/(fn\\?.*?)\"";
            return $"{prefix}/{Regex.Match(content1, regex).Groups[1].Value}";
        }
        public static async Task<string> GetSign(string url)
        {
            string content = await HttpClient.GetStringAsync(url);
            string regex = "'sign':'(.*?)'";
            return Regex.Match(content, regex).Groups[1].Value;
        }
        public static async Task<LanzouResponse> GetLanzouResponse(string referer,string sign)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{prefix}/ajaxm.php");
            request.Headers.Accept.ParseAdd("application/json, text/javascript, */*");
            request.Headers.AcceptLanguage.ParseAdd("zh-CN,zh;q=0.9,ko;q=0.8");
            request.Headers.Referrer = new Uri(referer);
            request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36");
            request.Content = new StringContent($"action=downprocess&sign={sign}&ves=1", Encoding.UTF8, "application/x-www-form-urlencoded");
            HttpResponseMessage response = await HttpClient.SendAsync(request);
            HttpContent content = response.Content;
            string data = (await content.ReadAsStringAsync()).Replace("\\/", "/");
            return JsonConvert.DeserializeObject<LanzouResponse>(data);
        }
        public static async Task<string> GetRealURL(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Head, url);
            request.Headers.AcceptLanguage.ParseAdd("zh-CN,zh;q=0.9,ko;q=0.8");
            var response = await HttpClient.SendAsync(request);
            return response.Headers.Location.OriginalString;
        }
    }
    public class LanzouResponse
    {
        public int zt;
        public string dom = string.Empty;
        public string url = string.Empty;
        public int inf;
    }
}
