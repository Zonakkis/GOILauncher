using System;
using System.Collections.Generic;
using System.Net.Http;
using System.Text.Json;
using System.Threading.Tasks;

namespace GOILauncher.Services.LeanCloud;

public class LeanCloudQuery<T>
{
    private readonly string _className;
    private string _objectId = string.Empty;
    private string? _order;
    private readonly List<string> _keys = [];
    private readonly Dictionary<string,object> _condition = [];
    private int _limit = 1000;
    private bool _count;

    public LeanCloudQuery()
    {
        _className = typeof(T).Name;
        Deselect("createdAt","updatedAt","objectId");
    }

    public async Task<string> Build()
    {
        var queryParams = new Dictionary<string,string>();
        if (_order is not null)
        {
            queryParams.Add("order", _order);
        }
        if (_keys.Count > 0)
        {
            queryParams.Add("keys", string.Join(',', _keys));
        }
        if (_condition.Count > 0)
        {
            queryParams.Add("where", JsonSerializer.Serialize(_condition));
        }
        if (_count)
        {
            queryParams.Add("count", "1");
        }
        queryParams.Add("limit", _limit.ToString());
        return $"{_className}{_objectId}?{await new FormUrlEncodedContent(queryParams).ReadAsStringAsync()}";
    }
    public LeanCloudQuery<T> Get(string objectId)
    {
        _objectId = $"/{objectId}";
        return this;
    }
    public LeanCloudQuery<T> OrderByAscending(string key)
    {
        _order = key;
        return this;
    }
    public LeanCloudQuery<T> OrderByDescending(string key)
    {
        _order = $"-{key}";
        return this;
    }
    public LeanCloudQuery<T> Select(params ReadOnlySpan<string> keys)
    {
        _keys.AddRange(keys);
        return this;
    }
    public LeanCloudQuery<T> Deselect(params ReadOnlySpan<string> keys)
    {
        foreach (var key in keys)
        {
            _keys.Add($"-{key}");
        }
        return this;
    }
    public LeanCloudQuery<T> Where(string key, object value)
    {
        _condition.Add(key, value);
        return this;
    }
    public LeanCloudQuery<T> Limit(int limit)
    {
        _limit = limit;
        return this;
    }
    public LeanCloudQuery<T> Count()
    {
        _count = true;
        _limit = 0;
        return this;
    }
}