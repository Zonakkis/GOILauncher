using Avalonia.Controls;
using Avalonia.Platform.Storage;
using System;
using System.Collections.Generic;
using System.IO;
using System.IO.Compression;
using System.Text;
using System.Text.Json;
using System.Threading.Tasks;

namespace GOILauncher.Services;

public class FileService(Lazy<TopLevel> topLevel)
{
    public async Task<IStorageFolder?> OpenFolderPickerAsync(string title)
    {
        var folderPickerOpenOptions = new FolderPickerOpenOptions
        {
            Title = title,
            AllowMultiple = false
        };
        var folder = await topLevel.Value.StorageProvider.OpenFolderPickerAsync(folderPickerOpenOptions);
        return folder.Count > 0 ? folder[0] : null;
    }
    public static void SaveAsJson(string path, string fileName, object obj, bool overwrite = true)
    {
        using StreamWriter streamWriter = new(Path.Combine(path, fileName), !overwrite);
        var json = JsonSerializer.Serialize(obj);
        streamWriter.WriteLine(json);
    }

    public static T LoadFromJson<T>(string path, string fileName)
    {
        using StreamReader streamReader = new(Path.Combine(path, fileName));
        var json = streamReader.ReadToEnd();
        return JsonSerializer.Deserialize<T>(json);
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
    public static void ExtractZip(string zipPath, string destinationPath, bool deleteAfterExtract)
    {
        using (var zipArchive = ZipFile.OpenRead(zipPath))
        {
            foreach (var zipArchiveEntry in zipArchive.Entries)
            {
                var filePath = Path.Combine(destinationPath, zipArchiveEntry.FullName);

                // 如果条目是文件而非文件夹，则解压
                if (string.IsNullOrEmpty(zipArchiveEntry.Name))
                {
                    // 是文件夹，跳过
                    continue;
                }
                // 创建文件夹
                Directory.CreateDirectory(Path.GetDirectoryName(filePath)!);
                // 解压文件
                zipArchiveEntry.ExtractToFile(filePath, overwrite: true);
            }
        }
        if (deleteAfterExtract)
        {
            File.Delete(zipPath);
        }
    }
}