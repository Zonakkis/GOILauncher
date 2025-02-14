using Avalonia.Controls;
using Avalonia.Platform.Storage;
using Ionic.Zip;
using LC.Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;

namespace GOILauncher.Services;

public class FileService(TopLevel topLevel)
{
    public async Task<IStorageFolder?> OpenFolderPickerAsync(string title)
    {
        var folderPickerOpenOptions = new FolderPickerOpenOptions
        {
            Title = title,
            AllowMultiple = false
        };
        var folder = await topLevel.StorageProvider.OpenFolderPickerAsync(folderPickerOpenOptions);
        return folder.Count > 0 ? folder[0] : null;
    }
    public static void SaveAsJson(string path, string fileName, object obj, bool overwrite = true)
    {
        using StreamWriter streamWriter = new(Path.Combine(path, fileName), !overwrite);
        streamWriter.WriteLine(JsonConvert.SerializeObject(obj));
    }

    public static T LoadFromJson<T>(string path, string fileName)
    {
        using StreamReader streamReader = new(Path.Combine(path, fileName));
        var json = streamReader.ReadToEnd();
        return JsonConvert.DeserializeObject<T>(json);
    }
    public static async Task CombineZipSegment(string path, string zipName, string searchPattern)
    {
        if (File.Exists(zipName))
        {
            File.Delete(zipName);
        }
        List<string> zipFiles = [.. Directory.GetFiles(path, searchPattern)];
        if (zipFiles.Count == 1)
        {
            File.Move(zipFiles[0], zipName);
            return;
        }

        await using Stream zipStream = File.OpenWrite(Path.Combine(path, zipName));
        foreach (var zipFile in zipFiles)
        {
            await using (Stream stream = File.OpenRead(zipFile))
            {
                await Task.Run(() => stream.CopyTo(zipStream));
            }
            File.Delete(zipFile);
        }
    }
    public static async Task ExtractZip(string zipPath, string destinationPath, bool deleteAfterExtract, EventHandler<ExtractProgressEventArgs>? progressHandler = null)
    {
        using (var zip = ZipFile.Read(zipPath, new ReadOptions() { Encoding = Encoding.GetEncoding("GBK") }))
        {
            if (progressHandler is not null)
            {
                zip.ExtractProgress += progressHandler;
            }
            foreach (var entry in zip)
            {

                await Task.Run(() => entry.Extract(destinationPath, ExtractExistingFileAction.OverwriteSilently));
            }
        }
        if (deleteAfterExtract)
        {
            File.Delete(zipPath);
        }
    }
}