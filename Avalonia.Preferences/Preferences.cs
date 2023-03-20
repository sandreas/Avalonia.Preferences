using Avalonia.Preferences.Storage;

namespace Avalonia.Preferences;

public class Preferences: IPreferences
{
    private static IPreferencesStorage? _storage;

    public Preferences(IPreferencesStorage? fallbackStorage = null)
    {
        _storage ??= fallbackStorage ?? new GenericPreferencesStorage();
    }
    
    public static void Init(IPreferencesStorage preferences)
    {
        _storage = preferences;
    }

    public bool Set<T>(string key, T? value) => _storage?.Set(key, value) ?? false;


    public T? Get<T>(string key, T? defaultValue) => _storage == null ? default : _storage.Get(key, defaultValue);

    public bool Remove(string key) => _storage?.Remove(key) ?? false;

    public int Clear() => _storage?.Clear() ?? -1;

    public bool ContainsKey(string key) => _storage?.ContainsKey(key) ?? false;
}