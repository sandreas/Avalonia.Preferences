namespace Avalonia.Preferences;

internal static class AsyncHelper
{
    private static readonly TaskFactory Factory = new(CancellationToken.None, 
            TaskCreationOptions.None, 
            TaskContinuationOptions.None, 
            TaskScheduler.Default);

    public static TResult RunSync<TResult>( Func<Task<TResult>> func)
    {
        return Factory
            .StartNew(func)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }

    public static void RunSync( Func<Task> func)
    {
        Factory
            .StartNew(func)
            .Unwrap()
            .GetAwaiter()
            .GetResult();
    }
}
