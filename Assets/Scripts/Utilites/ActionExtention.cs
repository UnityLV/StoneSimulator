using System;
using System.Threading.Tasks;

public static class ActionExtention
{
    public static async void InvokeWithDelay(this Action action,float delay = 1)
    {
        await Task.Delay(TimeSpan.FromSeconds(delay));
        action?.Invoke();
    }
}