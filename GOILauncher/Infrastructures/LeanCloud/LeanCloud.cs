using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;
using System.Text.Json;
using System.Text;
using GOILauncher.Infrastructures.Interfaces;

namespace GOILauncher.Infrastructures.LeanCloud;

public class LeanCloud : ILeanCloud
{
    private readonly HttpClient _httpClient;
    public LeanCloud(string url, string appId, string appKey, HttpClient httpClient)
    {
        httpClient.BaseAddress = new Uri(url);
        httpClient.DefaultRequestHeaders.Add("X-LC-Id", appId);
        httpClient.DefaultRequestHeaders.Add("X-LC-Key", appKey);
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
        var query = new LeanCloudQuery<T>()
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
}