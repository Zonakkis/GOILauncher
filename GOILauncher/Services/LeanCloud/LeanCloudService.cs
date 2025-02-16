using System;
using GOILauncher.Models;
using LeanCloud;
using LeanCloud.Storage;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;
using System.Diagnostics;
using System.Text.Json;
using System.Collections.ObjectModel;

namespace GOILauncher.Services.LeanCloud;

public class LeanCloudService
{
    public LeanCloudService(HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri("https://3dec7zyj.lc-cn-n1-shared.com/1.1/classes/");
        httpClient.DefaultRequestHeaders.Add("X-LC-Id", "3Dec7Zyj4zLNDU0XukGcAYEk-gzGzoHsz");
        httpClient.DefaultRequestHeaders.Add("X-LC-Key", "uHF3AdKD4i3RqZB7w1APiFRF");
        _httpClient = httpClient;
    }
    public async Task<List<T>> Find<T>(LeanCloudQuery leanCloudQuery)
    {
        var httpResponseMessage = await _httpClient.GetAsync(await leanCloudQuery.Build());
        httpResponseMessage.EnsureSuccessStatusCode();
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(content);
        var results = doc.RootElement.GetProperty("results").ToString();
        return JsonSerializer.Deserialize<List<T>>(results)!;
    }
    public async Task<ObservableCollection<T>> FindAsObservableCollection<T>(LeanCloudQuery leanCloudQuery)
    {
        var httpResponseMessage = await _httpClient.GetAsync(await leanCloudQuery.Build());
        httpResponseMessage.EnsureSuccessStatusCode();
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(content);
        var results = doc.RootElement.GetProperty("results").ToString();
        return JsonSerializer.Deserialize<ObservableCollection<T>>(results)!;
    }
    public async Task<ObservableCollection<Mod>> GetMods(string modName)
    {
        var query = new LeanCloudQuery(modName)
                        .OrderByDescending("Build")
                        .Select("Name", "Author", "Build", "Url", "TargetGameVersion");
        return await FindAsObservableCollection<Mod>(query);
    }
    private readonly HttpClient _httpClient;
}