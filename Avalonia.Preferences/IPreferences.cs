namespace Avalonia.Preferences;

public interface IPreferences
{
    public bool Set<T>(string key, T? value);

    public T? Get<T>(string key, T? defaultValue);
    
    public bool Remove(string key);
    public int Clear();
    public bool ContainsKey(string key);
    
    public Task<bool> SetAsync<T>(string key, T? value, CancellationToken? ct = null);

    public Task<T?> GetAsync<T>(string key, T? defaultValue, CancellationToken? ct = null);
    
    public Task<bool> RemoveAsync(string key, CancellationToken? ct = null);
    public Task<int> ClearAsync(CancellationToken? ct = null);
}