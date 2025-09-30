# Avalonia.Preferences
Cross platform preferences library for AvaloniaUI

IMPORTANT: There is a newer library with a sligthtly different API, that also supports WASM and is generally better in terms of compatibility to official libraries. I'd recommend to no longer use THIS respository / library (`Avalonia.Preferences`) and instead use the new one instead:

https://github.com/sandreas/Avalonia.SimplePreferences


## Usage

- Install nuget `Sandreas.Avalonia.Preferences`


### Dependency Injection

```c#
var services = new ServiceCollection();
// ...        
services.AddSingleton<Preferences>();
// ...        
```

### API sample (simple)


```c#

var counter = 0;

// check for key
if (preferences.ContainsKey("counter"))
{
    // get value with defaultValue fallback
    counter = _preferences.Get("counter", 0);
}


counter++;

// set value and check for success
if(!_preferences.Set("counter", counter)) {
    Console.WriteLine("Error: Could not set counter");
}

// remove value
if(!_preferences.Remove("counter")) {
    Console.WriteLine("Error: Could not remove counter");
}

// remove all values (clear)
var clearedItemsCount = _preferences.Clear();
if(clearedItemsCount == -1) {
    Console.WriteLine("Error: Could not clear preferences");
} else {
    Console.WriteLine("Success: Removed " + clearedItemsCount + " items from preferences");
}
```



### API sample (async, xplat)

#### Platform specific storage
If you need platform specific storage (e.g. for iOS or Android), you have to implement your own `IPreferences` implementation and statically set it before instantiating `Preferences`.
To simplify the implementation, you can extend `AbstractPreferencesStorage` already providing some helpful overridable methods. As an example take a look at `GenericPreferencesStorage` 
```c#
    // e.g. YourProject.Android/SplashActivity.cs
    protected override void OnResume()
    {
        base.OnResume();
        // your platform specific IPreferences implementation must be added statically before instantiation
        Preferences.PlatformStorage = new AndroidPlatformStorage(Application.Context);
        StartActivity(new Intent(Application.Context, typeof(MainActivity)));
    }
```

#### Async usage
```c#
// _preferences is set as class property via Dependency Injection
private async Task<int> GetCounterAsync() {    
    // cancellation is not actually used, but you could
    var cts = new CancellationTokenSource(); 
    var ct =  cts.Token;
    var counter = 0;

    // check for key
    if (_preferences.ContainsKey("counter"))
    {
        // get value with defaultValue fallback
        counter = await _preferences.GetAsync("counter", 0, ct);
    }


    counter++;

    // set value and check for success
    if(!await _preferences.SetAsync("counter", counter, ct)) {
        Console.WriteLine("Error: Could not set counter");
    }

    // remove value
    if(!await _preferences.RemoveAsync("counter", ct)) {
        Console.WriteLine("Error: Could not remove counter");
    }

    // remove all values (clear)
    var clearedItemsCount = await _preferences.ClearAsync(ct);
    if(clearedItemsCount == -1) {
        Console.WriteLine("Error: Could not clear preferences");
    } else {
        Console.WriteLine("Success: Removed " + clearedItemsCount + " items from preferences");
    }
    return counter;
}
```
