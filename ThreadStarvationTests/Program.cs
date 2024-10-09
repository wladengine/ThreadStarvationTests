using System.Diagnostics;

namespace ThreadStarvationDemo;

static class Program
{
    static int count = 0;
    static int collector = 0;
    static readonly Stopwatch stopwatch = new();

    private static void Main(string[] args)
    {
        Console.WriteLine($"PID: {Environment.ProcessId}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();

        Console.WriteLine("Process has started...");

        stopwatch.Start();

        var tasks = Enumerable.Range(1, 100_000).Select(x => DoWorkAsync()).ToArray();

        Task.WaitAll(tasks);

        Console.WriteLine("Done!");
        Console.ReadKey();
    }

    #region Worker Methods
    static async Task DoWork()
    {
        int c = await Task.Run(DbProvider.GetDataFromDB);
        Interlocked.Add(ref collector, c);
        LogProgress(Interlocked.Increment(ref count));
    }

    static async Task DoWorkAsync()
    {
        int c = await DbProvider.GetDataFromDBAsync();
        Interlocked.Add(ref collector, c);
        LogProgress(Interlocked.Increment(ref count));
    }

    static void LogProgress(int c)
    {
        if (c % 1_000 == 0)
        {
            Console.WriteLine($"Progress: {c,6}, Time: {stopwatch.Elapsed}, RPS: {1_000.0/stopwatch.Elapsed.TotalSeconds:####.##}");
            stopwatch.Restart();
        }
    }
    #endregion
}