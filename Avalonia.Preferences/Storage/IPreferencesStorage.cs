namespace Avalonia.Preferences.Storage;

public interface IPreferencesStorage : IPreferences
{
    protected bool SetSerialized(string key, string value);
    
    protected string GetSerialized(string key);

}