namespace ThreadStarvationDemo;

public static class DbProvider
{
    #if DEBUG
    private const int Delay = 1;
    #else
    private const int Delay = 10;
    #endif

    public static int GetDataFromDb()
    {
        Thread.Sleep(Delay);
        return Random.Shared.Next(10);
    }

    public static async Task<int> GetDataFromDbAsync()
    {
        await Task.Delay(Delay);
        return Random.Shared.Next(10);
    }
}
