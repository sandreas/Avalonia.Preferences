# Avalonia.Preferences
Cross platform preferences library for AvaloniaUI


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
    // get value
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
```c#
    // your platform specific IPreferencesStorage implementation must be added statically before instantiation
    // e.g. YourProject.Android/SplashActivity.cs
    protected override void OnResume()
    {
        base.OnResume();
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
        // get value
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