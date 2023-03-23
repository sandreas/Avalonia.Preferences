namespace Avalonia.Preferences.Storage;

public abstract class AbstractPreferencesStorage: IPreferences
{
    protected static bool HasTokenBeenCancelled(CancellationToken? ct)
    {
        if (ct == null)
        {
            return false;
        }

        return ct.Value.CanBeCanceled && ct.Value.IsCancellationRequested;
    }
    
    public bool Set<T>(string key, T? value) => AsyncHelper.RunSync(() =>  SetAsync(key, value));
    public async Task<bool> SetAsync<T>(string key, T? value, CancellationToken? ct = null) => await TryPersistAsync(key, value, ct??CancellationToken.None);
    public T? Get<T>(string key, T? defaultValue) =>  AsyncHelper.RunSync(() =>  GetAsync(key, defaultValue));
    public async Task<T?> GetAsync<T>(string key, T? defaultValue, CancellationToken? ct = null) =>
        !ContainsKey(key) ? defaultValue : await LoadAsync<T?>(key, ct ?? CancellationToken.None);
    public abstract Task<bool> RemoveAsync(string key, CancellationToken? ct = null);
    public abstract Task<int> ClearAsync(CancellationToken? ct = null);
    
    public bool Remove(string key) => AsyncHelper.RunSync(() =>  RemoveAsync(key));
    public int Clear()  => AsyncHelper.RunSync(() => ClearAsync());
    public abstract bool ContainsKey(string key);

    public abstract Task<bool> TryPersistAsync<T>(string key, T value, CancellationToken ct);
    public abstract Task<T?> LoadAsync<T>(string key, CancellationToken ct);

}