namespace Avalonia.Preferences.Storage;

public interface IPreferencesStorage : IPreferences
{
    public Stream OpenReadStream(string key);
    public Stream OpenWriteStream(string key);
}