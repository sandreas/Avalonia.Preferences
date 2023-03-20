
using System.Text.Json;

namespace Avalonia.Preferences.Storage;

public abstract class AbstractPreferencesStorage: IPreferencesStorage
{
    protected virtual string Serialize<T>(T value) => JsonSerializer.Serialize(value);

    protected virtual T? Deserialize<T>(string value) => JsonSerializer.Deserialize<T>(value);
    public bool Set<T>(string key, T? value) => SetSerialized(key, Serialize(value));
    public T? Get<T>(string key, T? defaultValue) => !ContainsKey(key) ? defaultValue : Deserialize<T?>(GetSerialized(key));
    
    public abstract bool SetSerialized(string key, string value);
    public abstract string GetSerialized(string key);
    public abstract bool Remove(string key);
    public abstract int Clear();
    public abstract bool ContainsKey(string key);
}