using LeanCloud.Storage;
using System.Collections.Generic;
using System.Net.Http;
using System.Reflection.Emit;
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
        if (_keys.Count > 0)
        {
            queryParams.Add("keys", string.Join(',', _keys));
        }
        if (_condition.Count > 0)
        {
            queryParams.Add("where", JsonSerializer.Serialize(_condition));
        }
        return $"{_className}{(string.IsNullOrEmpty(_objectId)?string.Empty:"/")}{_objectId}?{await new FormUrlEncodedContent(queryParams).ReadAsStringAsync()}";
    }
    public LeanCloudQuery<T> Get(string objectId)
    {
        _objectId = objectId;
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

    public LeanCloudQuery<T> Where(string key, object value)
    {
        _condition.Add(key, value);
        return this;
    }
}