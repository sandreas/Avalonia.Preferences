using Avalonia.Preferences.Storage;

namespace Avalonia.Preferences.Android.Storage;
/*
// https://github.com/jamesmontemagno/SettingsPlugin/blob/master/src/Plugin.Settings/Settings.android.cs
public class AndroidPreferencesStorage : AbstractPreferencesStorage
{
    private readonly ISharedPreferences _sharedPreferences;

    public AndroidPreferencesStorage(Context appContext)
    {
        _sharedPreferences = appContext.GetSharedPreferences(
            appContext.PackageName , FileCreationMode.Private)!;
    }

    public override bool SetSerialized(string key, string value)
    {
        using var editor = _sharedPreferences.Edit();
        if (editor == null)
        {
            return false;
        }
        editor.PutString(key, Convert.ToString(value, CultureInfo.InvariantCulture));
        return true;
    }

    public override string GetSerialized(string key) => _sharedPreferences.GetString(key, "") ?? "";


    public override bool Remove(string key)
    {
        using var editor = _sharedPreferences.Edit();
        if (editor == null)
        {
            return false;
        }
        editor.Remove(key);
        return true;
    }

    public override int Clear()
    {
        var count = _sharedPreferences.All?.Count ?? 0;
        using var editor = _sharedPreferences.Edit();
        if (editor == null)
        {
            return -1;
        }

        editor.Clear();
        editor.Commit();
        return count;
    }

    public override bool ContainsKey(string key) => _sharedPreferences.Contains(key);
}
*/