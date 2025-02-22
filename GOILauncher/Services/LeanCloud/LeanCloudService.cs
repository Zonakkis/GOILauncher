using System;
using GOILauncher.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
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
    public async Task<T> Get<T>(string className, string objectId)
    {
        var query = new LeanCloudQuery<T>(className)
                        .Get(objectId);
        var httpResponseMessage = await _httpClient.GetAsync(await query.Build());
        httpResponseMessage.EnsureSuccessStatusCode();
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content)!;
    }
    public async Task<List<T>> Find<T>(LeanCloudQuery<T> leanCloudQuery)
    {
        var httpResponseMessage = await _httpClient.GetAsync(await leanCloudQuery.Build());
        httpResponseMessage.EnsureSuccessStatusCode();
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(content);
        var results = doc.RootElement.GetProperty("results").ToString();
        return JsonSerializer.Deserialize<List<T>>(results)!;
    }
    public async Task<ObservableCollection<T>> FindAsObservableCollection<T>(LeanCloudQuery<T> leanCloudQuery)
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
        var query = new LeanCloudQuery<Mod>(modName)
                        .OrderByDescending(nameof(Mod.Build))
                        .Select("Name", "Author", "Build", "Url", "TargetGameVersion");
        return await FindAsObservableCollection(query);
    }
    public async Task<ObservableCollection<Map>> GetMaps()
    {
        var query = new LeanCloudQuery<Map>(nameof(Map))
                        .OrderByAscending(nameof(Map.Name))
                        .Where("Platform", "PC")
                        .Select("Name", "Author", "Size", "Preview", "Url", "Difficulty", "Form", "Style");
        return await FindAsObservableCollection(query);
    }
    public async Task<List<Credit>> GetCredits()
    {
        var query = new LeanCloudQuery<Credit>(nameof(Credit))
                        .OrderByAscending(nameof(Credit.Player))
                        .Select("Player");
        return await Find(query);
    }
    private readonly HttpClient _httpClient;
}