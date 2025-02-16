using GOILauncher.ViewModels;
using System;
using System.Diagnostics;
using System.Net.Http;
using System.Text;
using System.Text.Json;
using System.Text.RegularExpressions;
using System.Threading.Tasks;

namespace GOILauncher.Helpers
{
    internal class LanzouyunHelper
    {
        public static string Prefix { get; set; } = string.Empty;
        public static HttpClient HttpClient => MainWindowViewModel.HttpClient;
        public static async Task<string?> GetDirectUrlAsync(string code)
        {
            try
            {
                var url = $"{Prefix}/{code}";
                url = await GetDownloadPage(url);
                var sign = await GetSign(url);
                var lanzouResponse = await GetLanzouResponse(url , sign);
                return await GetRealUrl($"{lanzouResponse.dom}/file/{lanzouResponse.url}");
            }
            catch (Exception ex)
            {
                _ = NotificationHelper.ShowNotification("发生错误！", $"获取直链失败：{ex.Message}", FluentAvalonia.UI.Controls.InfoBarSeverity.Error);
                Trace.WriteLine($"获取直链失败：{ex.Message}");
                return await GetDirectUrlAsync(code);
            }
        }
        public static async Task<string> GetDownloadPage(string url)
        {
            string content1 = await HttpClient.GetStringAsync(url);
            string regex = "\"/(fn\\?.*?)\"";
            return $"{Prefix}/{Regex.Match(content1, regex).Groups[1].Value}";
        }
        public static async Task<string> GetSign(string url)
        {
            string content = await HttpClient.GetStringAsync(url);
            string regex = "'sign':'(.*?)'";
            return Regex.Match(content, regex).Groups[1].Value;
        }
        public static async Task<LanzouResponse> GetLanzouResponse(string referer,string sign)
        {
            var request = new HttpRequestMessage(HttpMethod.Post, $"{Prefix}/ajaxm.php");
            request.Headers.Accept.ParseAdd("application/json, text/javascript, */*");
            request.Headers.AcceptLanguage.ParseAdd("zh-CN,zh;q=0.9,ko;q=0.8");
            request.Headers.Referrer = new Uri(referer);
            request.Headers.UserAgent.ParseAdd("Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36");
            request.Content = new StringContent($"action=downprocess&sign={sign}&ves=1", Encoding.UTF8, "application/x-www-form-urlencoded");
            var response = await HttpClient.SendAsync(request);
            var content = response.Content;
            var data = (await content.ReadAsStringAsync()).Replace("\\/", "/");
            return JsonSerializer.Deserialize<LanzouResponse>(data);
        }
        public static async Task<string?> GetRealUrl(string url)
        {
            var request = new HttpRequestMessage(HttpMethod.Head, url);
            request.Headers.AcceptLanguage.ParseAdd("zh-CN,zh;q=0.9,ko;q=0.8");
            var response = await HttpClient.SendAsync(request);
            return response.Headers.Location?.OriginalString;
        }
    }
    public class LanzouResponse
    {
        public int zt;
        public string dom = string.Empty;
        public string url = string.Empty;
    }
}
