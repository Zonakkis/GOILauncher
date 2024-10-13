using Downloader;
using LeanCloud;
using LeanCloud.Storage;
using System.IO;
using System.Net;
using System;
using System.Reflection;
using Ionic.Zip;
using System.Text;
using System.Diagnostics;

namespace GOILUpdater
{
    internal class Program
    {
        static void Main(string[] args)
        {
            bool exited = false;
            while (!exited)
            {
                if (Process.GetProcessesByName("GOILauncher").Length == 0)
                {
                    exited = true;
                }
                Thread.Sleep(100);
            }
            Console.WriteLine("下载中");
            WebClient webClient = new();
            webClient.DownloadFile(args[0], "GOILauncher.zip");
            //webClient.DownloadFile("http://lc-3Dec7Zyj.cn-n1.lcfile.com/DdpBYgVeVx9feNJiFyGq6XhdXQe0J9dB/GOIL.zip", "GOIL.zip");
            Console.WriteLine("解压中");
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            using (ZipFile zip = ZipFile.Read($"{Environment.CurrentDirectory}/GOILauncher.zip", new ReadOptions() { Encoding = Encoding.GetEncoding("GBK") }))
            {
                foreach (ZipEntry entry in zip)
                {
                    entry.Extract(ExtractExistingFileAction.OverwriteSilently);
                }
            }
            File.Delete("GOILauncher.zip");
            Process.Start("GOILauncher.exe");
        }

    }
}
