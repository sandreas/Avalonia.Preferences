using Avalonia.Preferences.Storage;

namespace Avalonia.Preferences;

public class Preferences : IPreferences
{
    public static IPreferences? PlatformStorage { get; set; }
    private readonly IPreferences _storage;

    public Preferences(IPreferences? storage = null)
    {
        _storage = PlatformStorage ?? storage ?? new GenericPreferencesStorage();
    }


    public bool Set<T>(string key, T? value) => _storage.Set(key, value);


    public T? Get<T>(string key, T? defaultValue) => _storage.Get(key, defaultValue);

    public bool Remove(string key) => _storage.Remove(key);

    public int Clear() => _storage.Clear();

    public bool ContainsKey(string key) => _storage.ContainsKey(key);

    public async Task<bool> SetAsync<T>(string key, T? value, CancellationToken? ct = null) =>
        await _storage.SetAsync(key, value, ct);

    public async Task<T?> GetAsync<T>(string key, T? defaultValue, CancellationToken? ct = null) =>
        await _storage.GetAsync(key, defaultValue, ct);

    public async Task<bool> RemoveAsync(string key, CancellationToken? ct = null) =>
        await _storage.RemoveAsync(key, ct);

    public async Task<int> ClearAsync(CancellationToken? ct = null) => await _storage.ClearAsync(ct);
}