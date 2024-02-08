using LC.Newtonsoft.Json;
using LC.Newtonsoft.Json.Linq;
using Microsoft.VisualBasic;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Net;
using System.Net.Http;
using System.Text;
using System.Threading.Tasks;

namespace GOI地图管理器.Helpers
{
    internal class LanzouyunDownloadHelper
    {
        public static async Task<string> GetDirectURL(string LanzouyunURL)
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












            string text = new WebClient().DownloadString(LanzouyunURL).Split(new string[] { "src=\"", "\" frameborder" }, StringSplitOptions.RemoveEmptyEntries)[3];
            string text2 = "https://wwn.lanzouv.com" + text;
            Trace.WriteLine(text2);
            string text3 = new WebClient().DownloadString(text2).Split(new string[] { "'sign':'" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { "','" }, StringSplitOptions.RemoveEmptyEntries)[0];
            Trace.WriteLine(text3);
            HttpWebRequest httpWebRequest = (HttpWebRequest)WebRequest.Create("https://wwn.lanzouv.com/ajaxm.php");
            httpWebRequest.Method = "POST";
            httpWebRequest.Accept = "application/json, text/javascript, */*";
            httpWebRequest.Headers.Add("accept-language", "zh-CN,zh;q=0.9,ko;q=0.8");
            httpWebRequest.ContentType = "application/x-www-form-urlencoded";
            httpWebRequest.Referer = text2;
            httpWebRequest.UserAgent = "Mozilla/5.0 (Windows NT 6.1; WOW64) AppleWebKit/537.36 (KHTML, like Gecko) Chrome/86.0.4240.198 Safari/537.36";
            string text5 = "action=downprocess&sign=" + text3 + "&ves=1";
            byte[] bytes = Encoding.UTF8.GetBytes(text5);
            httpWebRequest.GetRequestStream().Write(bytes, 0, bytes.Length);
            Stream responseStream = ((HttpWebResponse)httpWebRequest.GetResponse()).GetResponseStream();
            StreamReader streamReader = new StreamReader(responseStream, Encoding.GetEncoding("UTF-8"));
            string text6 = streamReader.ReadToEnd().Replace("\\/", "/");

            Trace.WriteLine(text6);
            LanzouResponse response = JsonConvert.DeserializeObject<LanzouResponse>(text6);
            string text7 = text6.Split(new string[] { "url\":\"" }, StringSplitOptions.RemoveEmptyEntries)[1].Split(new string[] { "\",\"inf" }, StringSplitOptions.RemoveEmptyEntries)[0];
            Trace.WriteLine(text7);
            HttpWebRequest httpWebRequest2 = (HttpWebRequest)WebRequest.Create($"{response.dom}/file/{response.url}");
            httpWebRequest2.Method = "HEAD";
            httpWebRequest2.AllowAutoRedirect = false;
            httpWebRequest2.Headers.Add("accept-language", "zh-CN,zh;q=0.9,ko;q=0.8");
            WebResponse webResponse = httpWebRequest2.GetResponse();
            string[] allKeys = webResponse.Headers.AllKeys;
            string text8 = webResponse.Headers.Get("Location");
            responseStream.Close();
            streamReader.Close();
            Trace.WriteLine(text8);
            return text8;
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
