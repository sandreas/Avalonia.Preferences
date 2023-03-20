using System;
using System.IO;
using System.IO.IsolatedStorage;

namespace Avalonia.Preferences.Storage;

// https://github.com/jamesmontemagno/SettingsPlugin/blob/master/src/Plugin.Settings/Settings.dotnet.cs
public class GenericPreferencesStorage : AbstractPreferencesStorage
{
    private static IsolatedStorageFile Store => IsolatedStorageFile.GetUserStoreForDomain();
    private readonly object _internalLock = new();

    public override bool SetSerialized(string key, string value)
    {
        lock (_internalLock)
        {
            try
            {
                using var stream = Store.OpenFile(key, FileMode.Create, FileAccess.Write);
                using var sw = new StreamWriter(stream);
                sw.Write(value);
                return true;
            }
            catch (Exception)
            {
                return false;
            }
        }
    }

    public override string GetSerialized(string key)
    {
        lock (_internalLock)
        {
            using var stream = Store.OpenFile(key, FileMode.Open);
            using var sr = new StreamReader(stream);
            return sr.ReadToEnd();
        }
    }

    public override bool Remove(string key)
    {
        lock (_internalLock)
        {
            if (ContainsKey(key))
            {
                Store.DeleteFile(key);
                return true;
            }

            return false;
        }
    }


    public override int Clear()
    {
        lock (_internalLock)
        {
            try
            {
                var fileNames = Store.GetFileNames();
                foreach (var file in fileNames)
                {
                    Store.DeleteFile(file);
                }

                return fileNames.Length;
            }
            catch (Exception)
            {
                return -1;
            }
        }
    }


    public override bool ContainsKey(string key) => Store.FileExists(key);
}