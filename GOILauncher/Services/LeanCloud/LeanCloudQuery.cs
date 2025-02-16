using System.Collections.Generic;
using System.Net.Http;
using System.Threading.Tasks;

namespace GOILauncher.Services.LeanCloud;

public class LeanCloudQuery<T>
{
    private readonly string _className;
    private string? _order;
    private readonly List<string> _keys = [];

    public LeanCloudQuery(string className)
    {
        _className = className;
        Deselect("createdAt","updatedAt","objectId");
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
    public LeanCloudQuery<T> Select(params string[] keys)
    {
        _keys.AddRange(keys);
        return this;
    }
    public LeanCloudQuery<T> Deselect(params string[] keys)
    {
        foreach (var key in keys)
        {
            _keys.Add($"-{key}");
        }
        return this;
    }
}