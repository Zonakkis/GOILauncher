using Downloader;
using GOILauncher.Models;
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
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace GOILauncher.Helpers
{
    internal class LanzouyunDownloadHelper
    {

        public static async Task DownloadMap(string url,string path,Map map,int id)
        {
            WebClient webClient = new WebClient();
            webClient.Credentials = CredentialCache.DefaultCredentials;
            //webClient.DownloadFileCompleted += DownloadFileCallback;
            //webClient.DownloadProgressChanged += map.OnDownloadProgressChanged;
            webClient.DownloadFileCompleted += map.OnDownloadCompleted;
            webClient.Headers.Add("ID", id.ToString());
            webClient.CachePolicy = new System.Net.Cache.RequestCachePolicy(System.Net.Cache.RequestCacheLevel.NoCacheNoStore);
            await webClient.DownloadFileTaskAsync(url, path);

            //using (HttpClient httpClient = new HttpClient())
            //{
            //    using (var response = await httpClient.GetAsync(url, HttpCompletionOption.ResponseHeadersRead))
            //    {
            //        using(var download = await response.Content.ReadAsStreamAsync())
            //        {
            //            await download.CopyToAsync(new FileStream(path,FileMode.Create),104857600);
            //        }
            //    }
            //}
        }

        public static async Task Download(string url,string fileName,EventHandler<DownloadStartedEventArgs> downloadStartedEvent, EventHandler<Downloader.DownloadProgressChangedEventArgs> downloadProgressChangedEventArgs, EventHandler<AsyncCompletedEventArgs>? downloadCompletedEvent,CancellationToken cancellationToken = default)
        {
            var downloadOpt = new DownloadConfiguration()
            {
                ChunkCount = 8,
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
            using (DownloadService downloadService = new DownloadService(downloadOpt))
            { 

                downloadService.DownloadStarted += downloadStartedEvent;
                downloadService.DownloadProgressChanged += downloadProgressChangedEventArgs;
                downloadService.DownloadFileCompleted += downloadCompletedEvent;
                await downloadService.DownloadFileTaskAsync(url, fileName, cancellationToken);
            }
        }

        public static async Task<string> GetDirectURLAsync(string LanzouyunURL)
        {
            //string url1;
            //string url2;
            //string url3;
            //using (HttpClient client = new HttpClient())
            //{
            //    using (HttpResponseMessage httpResponse = client.GetAsync(LanzouyunURL).Result)
            //    {
            //        using (HttpContent content = httpResponse.Content)
            //        {
            //            url2 = content.ReadAsStringAsync().Result.Split(new string[] { "src=\"", "\" frameborder" }, StringSplitOptions.RemoveEmptyEntries)[3];
            //            url2 = $"https://wwn.lanzouv.com{url2}";
            //            Trace.WriteLine(url2);
            //        }
            //    }
            //}
            //using (HttpClient client = new HttpClient())
            //{
            //    using (HttpResponseMessage httpResponse = client.GetAsync(url2).Result)
            //    {
            //        using (HttpContent content = httpResponse.Content)
            //        {
            //            url1 = content.ReadAsStringAsync().Result.Split(new string[] { "'sign':'" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { "','" }, StringSplitOptions.RemoveEmptyEntries)[0];
            //            Trace.WriteLine($"{url1}");
            //        }
            //    }
            //}
            //using (HttpClientHandler handler = new HttpClientHandler { UseDefaultCredentials = true })
            //{
            //    using (HttpClient client = new HttpClient(handler))
            //    {
            //        client.DefaultRequestHeaders.Referrer = new Uri(url2);
            //        url1 = $"action=downprocess&sign={url1}&ves=1";
            //        byte[] bytess = Encoding.UTF8.GetBytes(url1);
            //        await client.PostAsync("https://wwn.lanzouv.com/ajaxm.php", new StringContent(url1));
            //        HttpResponseMessage response = (await client.GetAsync("https://wwn.lanzouv.com/ajaxm.php"));
            //        StreamReader sr = new StreamReader(await response.Content.ReadAsStreamAsync(), Encoding.GetEncoding("UTF-8"));
            //        url3 = sr.ReadToEnd().Replace("\\/", "/");
            //        Trace.Write(url3);
            //    }
            //    using (HttpClientHandler handler1 = new HttpClientHandler { UseDefaultCredentials = true })
            //    {
            //        using (HttpClient client = new HttpClient(handler))
            //        {
            //            client.DefaultRequestHeaders.Add("Method", "HEAD");
            //            client.DefaultRequestHeaders.Add("AllowAutoRedirect", "false");
            //            HttpContent rm = (await client.GetAsync(url3.Substring(url3.Length - url3.Length + 15, url3.Length - 25).Replace("\",\"url\":\"", "/file/"))).Content;
            //            return rm.Headers.GetValues("location").ToString();
            //        }
            //    }

            //}

            //string text = new HttpClient().DownloadString(LanzouURL).Split(new string[] { "src=" }, StringSplitOptions.RemoveEmptyEntries)[2].Substring(1, 108);

            using (var webClient = new WebClient())
            {
                string htmlSource = (await webClient.DownloadStringTaskAsync(LanzouyunURL)).Split(new string[] { "src=\"", "\" frameborder" }, StringSplitOptions.RemoveEmptyEntries)[3];
                string url = "https://wwn.lanzouv.com" + htmlSource;
                Trace.WriteLine(url);
                string sign = (await webClient.DownloadStringTaskAsync(url)).Split(new string[] { "'sign':'" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { "','" }, StringSplitOptions.RemoveEmptyEntries)[0];
                Trace.WriteLine(sign);
                HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://wwn.lanzouv.com/ajaxm.php");
                httpWebRequest.Method = "POST";
                httpWebRequest.Accept = "application/json, text/javascript, */*";
                httpWebRequest.Headers.Add("accept-language", "zh-CN,zh;q=0.9,ko;q=0.8");
                httpWebRequest.ContentType = "application/x-www-form-urlencoded";
                httpWebRequest.Referer = url;
                httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36";
                string content = "action=downprocess&sign=" + sign + "&ves=1";
                byte[] bytes = Encoding.UTF8.GetBytes(content);
                (await httpWebRequest.GetRequestStreamAsync()).Write(bytes, 0, bytes.Length);
                using (Stream responseStream = ((HttpWebResponse)await httpWebRequest.GetResponseAsync()).GetResponseStream())
                {
                    using (StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8")))
                    {
                        string responseString = streamReader.ReadToEnd().Replace("\\/", "/");
                        Trace.WriteLine(responseString);
                        LanzouResponse response = JsonConvert.DeserializeObject<LanzouResponse>(responseString);
                        HttpWebRequest httpWebRequest2 = (HttpWebRequest)WebRequest.Create($"{response.dom}/file/{response.url}");
                        httpWebRequest2.Method = "HEAD";
                        httpWebRequest2.AllowAutoRedirect = false;
                        httpWebRequest2.Headers.Add("accept-language", "zh-CN,zh;q=0.9,ko;q=0.8");
                        WebResponse webResponse = await Task.Run(() => httpWebRequest2.GetResponse());
                        string[] allKeys = webResponse.Headers.AllKeys;
                        string directURL = webResponse.Headers.Get("Location")!;
                        Trace.WriteLine(directURL);
                        return directURL!;
                    }
                } 
            }
        }


        public static async Task<int> GetFileSizeAsync(string url)
        {
            HttpWebRequest httpWebRequest2 = (HttpWebRequest)WebRequest.Create(url);
            httpWebRequest2.Method = "HEAD";
            httpWebRequest2.AllowAutoRedirect = false;
            using (WebResponse webResponse = httpWebRequest2.GetResponse())
            {
                string contentLength = webResponse.Headers.Get("Content-Length");
                return Convert.ToInt32(contentLength);
            }
        }
    }



    public class LanzouResponse
    {
        public int zt;
        public string dom;
        public string url;
        public int inf;
    }
}
