using System.Text.Json;

namespace Avalonia.Preferences.Storage;

public abstract class AbstractPreferencesStorage: IPreferencesStorage
{
    protected virtual async Task<bool> SerializeAsync<T>(string key, T value, CancellationToken ct)
    {
        await using var stream = OpenWriteStream(key);
        try
        {
            await JsonSerializer.SerializeAsync(stream, value, (JsonSerializerOptions?)null, ct);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
    }

    protected virtual async Task<T?> DeserializeAsync<T>(string key, CancellationToken ct)
    {
        // it may happen, that a value type changes and can't be deserialized
        // so prevent exceptions in this case
        await using var stream = OpenReadStream(key);
        try
        {
            return await JsonSerializer.DeserializeAsync<T>(stream, (JsonSerializerOptions?)null, ct);
        }
        catch (Exception)
        {
            return default;
        }
    }

    protected bool HasTokenBeenCancelled(CancellationToken? ct)
    {
        if (ct == null)
        {
            return false;
        }

        return ct.Value.CanBeCanceled && ct.Value.IsCancellationRequested;
    }
    
    public bool Set<T>(string key, T? value) => AsyncHelper.RunSync(() =>  SetAsync(key, value));
    public async Task<bool> SetAsync<T>(string key, T? value, CancellationToken? ct = null) => await SerializeAsync(key, value, ct??CancellationToken.None);
    public T? Get<T>(string key, T? defaultValue) =>  AsyncHelper.RunSync(() =>  GetAsync(key, defaultValue));
    public async Task<T?> GetAsync<T>(string key, T? defaultValue, CancellationToken? ct = null) =>
        !ContainsKey(key) ? defaultValue : await DeserializeAsync<T?>(key, ct ?? CancellationToken.None);
    public abstract Task<bool> RemoveAsync(string key, CancellationToken? ct = null);
    public abstract Task<int> ClearAsync(CancellationToken? ct = null);
    
    public bool Remove(string key) => AsyncHelper.RunSync(() =>  RemoveAsync(key));
    public int Clear()  => AsyncHelper.RunSync(() => ClearAsync());
    public abstract bool ContainsKey(string key);
    public abstract Stream OpenReadStream(string key);
    public abstract Stream OpenWriteStream(string key);
}