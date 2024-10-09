using System.Runtime.CompilerServices;

namespace ThreadStarvationDemo;

public static class DbProvider
{
    const int Delay = 1;

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static int GetDataFromDB()
    {
        Thread.Sleep(Delay);
        return Random.Shared.Next();
    }

    [MethodImpl(MethodImplOptions.NoInlining)]
    public static async Task<int> GetDataFromDBAsync()
    {
        await Task.Delay(Delay);
        return Random.Shared.Next();
    }
}
