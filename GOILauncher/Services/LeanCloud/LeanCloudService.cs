using System;
using GOILauncher.Models;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Collections.ObjectModel;
using System.Text;

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
    public async Task Create<T>(T obj)
    {
        var json = JsonSerializer.Serialize(obj);
        var content = new StringContent(json, Encoding.UTF8, "application/json");
        var httpResponseMessage = await _httpClient.PostAsync(typeof(T).Name, content);
        httpResponseMessage.EnsureSuccessStatusCode();
    }
    public async Task<T> Get<T>(string objectId)
    {
        var query = new LeanCloudQuery<T>(typeof(T).Name)
                        .Get(objectId);
        var httpResponseMessage = await _httpClient.GetAsync(await query.Build());
        httpResponseMessage.EnsureSuccessStatusCode();
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        return JsonSerializer.Deserialize<T>(content)!;
    }
    public async Task<int> Count<T>(LeanCloudQuery<T> leanCloudQuery)
    {
        leanCloudQuery.Count();
        var httpResponseMessage = await _httpClient.GetAsync(await leanCloudQuery.Build());
        httpResponseMessage.EnsureSuccessStatusCode();
        var content = await httpResponseMessage.Content.ReadAsStringAsync();
        using var doc = JsonDocument.Parse(content);
        return doc.RootElement.GetProperty("count").GetInt32();
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
                        .Select(nameof(Mod.Name), nameof(Mod.Author), nameof(Mod.Build), 
                                nameof(Mod.Url), nameof(Mod.TargetGameVersion));
        return await FindAsObservableCollection(query);
    }
    public async Task<List<Map>> GetMaps()
    {
        var query = new LeanCloudQuery<Map>(nameof(Map))
                        .OrderByAscending(nameof(Map.Name))
                        .Where("Platform", "PC")
                        .Select("Name", "Author", "Size", "Preview", "Url", "Difficulty", "Form", "Style");
        return await Find(query);
    }
    public async Task<List<Speedrun>> GetSpeedruns()
    {
        var query = new LeanCloudQuery<Speedrun>(nameof(Speedrun))
                        .OrderByAscending("TotalTime")
                        .Where("Fastest", true)
                        .Where("Category", "Glitchless")
                        .Select(nameof(Speedrun.Player), nameof(Speedrun.UID), nameof(Speedrun.Country),
                                nameof(Speedrun.Platform), nameof(Speedrun.CountryCode),
                                nameof(Speedrun.Time), nameof(Speedrun.VID), nameof(Speedrun.VideoPlatform));
        return await Find(query);
    }
    public async Task<List<PendingRun>> GetPendingRuns()
    {
        var query = new LeanCloudQuery<PendingRun>(nameof(PendingRun))
                        .OrderByAscending("TotalTime")
                        .Select(nameof(PendingRun.Player), nameof(PendingRun.Category), 
                                nameof(Speedrun.Platform),nameof(PendingRun.Time), nameof(Speedrun.VID),
                                nameof(Speedrun.VideoPlatform));
        return await Find(query);
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