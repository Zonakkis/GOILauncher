using Downloader;
using LeanCloud;
using LeanCloud.Storage;
using System.IO;
using System.Net;
using System;
using System.Reflection;
using System.Text;
using System.Diagnostics;
using GOILUpdater.Helpers;
using System.Threading;
using System.IO.Compression;

namespace GOILUpdater
{
    internal class Program
    {
        static async Task Main(string[] args)
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
            await DownloadHelper.Download(Path.Combine(Directory.GetCurrentDirectory(), "GOILauncher.zip"), new Update(args[0]));
            Encoding.RegisterProvider(CodePagesEncodingProvider.Instance);
            Console.WriteLine("准备解压……");
            ZipFile.ExtractToDirectory(Path.Combine(Directory.GetCurrentDirectory(), "GOILauncher.zip"), Directory.GetCurrentDirectory(), Encoding.GetEncoding("GBK"), true);
            Console.WriteLine("解压完成");
            File.Delete("GOILauncher.zip");
            Process.Start("GOILauncher.exe");
        }
    }
}
