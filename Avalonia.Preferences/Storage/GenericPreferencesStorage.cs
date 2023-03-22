using System.IO.IsolatedStorage;

namespace Avalonia.Preferences.Storage;

// https://github.com/jamesmontemagno/SettingsPlugin/blob/master/src/Plugin.Settings/Settings.dotnet.cs
public class GenericPreferencesStorage : AbstractPreferencesStorage
{
    private static IsolatedStorageFile Store => IsolatedStorageFile.GetUserStoreForDomain();
    private static readonly SemaphoreSlim Sema = new(1, 1);

    public override bool ContainsKey(string key) => Store.FileExists(key);

    public override Stream OpenReadStream(string key) => Store.OpenFile(key, FileMode.Open);

    public override Stream OpenWriteStream(string key) => Store.OpenFile(key, FileMode.Create, FileAccess.Write);

    public override async Task<bool> RemoveAsync(string key, CancellationToken? ct = null)
    {
        await Sema.WaitAsync();
        try
        {
            if (!ContainsKey(key) || HasTokenBeenCancelled(ct)) return false;
            Store.DeleteFile(key);
            return true;
        }
        finally
        {
            Sema.Release();
        }
    }

    public override async Task<int> ClearAsync(CancellationToken? ct = null)
    {
        await Sema.WaitAsync();
        try
        {
            var fileNames = Store.GetFileNames();
            foreach (var file in fileNames)
            {
                if (HasTokenBeenCancelled(ct))
                {
                    return -1;
                }
                Store.DeleteFile(file);
            }

            return fileNames.Length;
        }
        catch (Exception)
        {
            return -1;
        }
        finally
        {
            Sema.Release();
        }
    }
    
    
}