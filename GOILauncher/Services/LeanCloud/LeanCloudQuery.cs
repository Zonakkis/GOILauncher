using LeanCloud.Storage;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http;
using System.Threading.Tasks;

namespace GOILauncher.Services.LeanCloud;

public class LeanCloudQuery
{
    private readonly string _className;
    private string? _order;
    private readonly List<string> _keys = [];

    public LeanCloudQuery(string className)
    {
        _className = className;
        Deselect("createdAt");
        Deselect("updatedAt");
        Deselect("objectId");
    }

    public async Task<string> Build()
    {
        var queryParams = new Dictionary<string,string>();
        if (_order is not null)
        {
            queryParams.Add("order", _order);
        }
        var keys = string.Join(',',_keys);
        if (!string.IsNullOrEmpty(keys))
        {
            queryParams.Add("keys", keys);
        }
        return $"{_className}?{await new FormUrlEncodedContent(queryParams).ReadAsStringAsync()}";
    }
    public LeanCloudQuery OrderByAscending(string key)
    {
        _order = key;
        return this;
    }
    public LeanCloudQuery OrderByDescending(string key)
    {
        _order = $"-{key}";
        return this;
    }
    public LeanCloudQuery Select(params string[] keys)
    {
        _keys.AddRange(keys);
        return this;
    }
    public LeanCloudQuery Deselect(params string[] keys)
    {
        foreach (var key in keys)
        {
            _keys.Add($"-{key}");
        }
        return this;
    }
}