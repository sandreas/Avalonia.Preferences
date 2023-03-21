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

### API sample

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
if(!_preferences.Set("counter", PrefTester)) {
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