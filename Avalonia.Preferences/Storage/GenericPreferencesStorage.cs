using System.IO.IsolatedStorage;
using System.Text.Json;

namespace Avalonia.Preferences.Storage;

// https://github.com/jamesmontemagno/SettingsPlugin/blob/master/src/Plugin.Settings/Settings.dotnet.cs
public class GenericPreferencesStorage : AbstractPreferencesStorage
{
    private static IsolatedStorageFile Store => IsolatedStorageFile.GetUserStoreForDomain();
    private static readonly SemaphoreSlim Sema = new(1, 1);

    public override bool ContainsKey(string key) => Store.FileExists(key);

    public override async Task<bool> RemoveAsync(string key, CancellationToken? ct = null)
    {
        await Sema.WaitAsync(ct ?? CancellationToken.None);
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
        await Sema.WaitAsync(ct ?? CancellationToken.None);
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
    
    public override async Task<bool> TryPersistAsync<T>(string key, T value, CancellationToken ct)
    {
        await Sema.WaitAsync(ct);
        try
        {
            await using var stream = Store.OpenFile(key, FileMode.Create, FileAccess.Write);
            await JsonSerializer.SerializeAsync(stream, value, (JsonSerializerOptions?)null, ct);
            return true;
        }
        catch (Exception)
        {
            return false;
        }
        finally
        {
            Sema.Release();
        }
    }

    public override async Task<T?> LoadAsync<T>(string key, CancellationToken ct) where T: default
    {
        await Sema.WaitAsync(ct);

        // it may happen, that a value type changes and can't be deserialized
        // so prevent exceptions in this case
        try
        {
            await using var stream = Store.OpenFile(key, FileMode.Open);
            return await JsonSerializer.DeserializeAsync<T>(stream, (JsonSerializerOptions?)null, ct);
        }
        catch (Exception)
        {
            return default;
        }
        finally
        {
            Sema.Release();
        }
    }
    
}