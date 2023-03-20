namespace Avalonia.Preferences;

public interface IPreferences
{
    public bool Set<T>(string key, T? value);

    public T? Get<T>(string key, T? defaultValue);
    
    public bool Remove(string key);
    public int Clear();
    public bool ContainsKey(string key);
}