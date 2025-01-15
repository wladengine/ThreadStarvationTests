using System.Diagnostics;

namespace ThreadStarvationDemo;

internal static class Program
{
    private static int _count = 0;
    private static int _collector = 0;
    private static readonly Stopwatch Stopwatch = new();

    private static void Main(string[] args)
    {
        Console.WriteLine($"PID: {Environment.ProcessId}");
        Console.WriteLine("Press any key to continue...");
        Console.ReadKey();

        Console.WriteLine("Process has started...");

        Stopwatch.Start();

        Task[] tasks = Enumerable.Range(1, 100_000)
            .Select(x => DoWork())
            .ToArray();

        Task.WaitAll(tasks);

        Console.WriteLine("Done!");
        Console.WriteLine($"Result: {_collector}");

        Console.ReadKey();
    }

    #region Worker Methods

    private static async Task DoWork()
    {
        int response = await Task.Run(DbProvider.GetDataFromDb);
        Interlocked.Add(ref _collector, response);
        LogProgress(Interlocked.Increment(ref _count));
    }

    static async Task DoWorkAsync()
    {
        int response = await DbProvider.GetDataFromDbAsync();
        Interlocked.Add(ref _collector, response);
        LogProgress(Interlocked.Increment(ref _count));
    }

    private static void LogProgress(int counter)
    {
        if (counter % 1_000 == 0)
        {
            Console.WriteLine($"Progress: {counter,6}, Time: {Stopwatch.Elapsed}, RPS: {1_000.0/Stopwatch.Elapsed.TotalSeconds:####.##}");
            Stopwatch.Restart();
        }
    }
    #endregion
}