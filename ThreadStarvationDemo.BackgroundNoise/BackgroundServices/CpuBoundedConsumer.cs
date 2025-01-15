using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ThreadStarvationDemo.BackgroundNoise.BackgroundServices;

public class CpuBoundedConsumer : BackgroundService
{
    private static int _counter = 0;
    private static readonly Stopwatch Stopwatch = new();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Stopwatch.Start();
        await foreach (int item in GetItemsAsync(stoppingToken))
        {
            LogProgress(Interlocked.Increment(ref _counter));

            // fire and forget
            Task.Factory.StartNew((i) => HandleAndSaveResults((int)i), item);
            //Task.Factory.StartNew((i) => HandleAndSaveResults((int)i), item, TaskCreationOptions.LongRunning);
        }
    }

    private static async IAsyncEnumerable<int> GetItemsAsync([EnumeratorCancellation] CancellationToken stoppingToken)
    {
        for (var i = 0; i < 1_000_000; i++)
        {
            await Task.Delay(50, stoppingToken);

            if (stoppingToken.IsCancellationRequested)
            {
                yield break;
            }

            yield return i;
        }
    }

    private static void LogProgress(int c)
    {
        if (c % 100 == 0)
        {
            Console.WriteLine($"Progress: {c,6}, Time: {Stopwatch.Elapsed}, OPS: {100.0 / Stopwatch.Elapsed.TotalSeconds:####.##}");
            Stopwatch.Restart();
        }
    }

    private static void HandleAndSaveResults(int item)
    {
        // Simulate CPU-bound work
        Thread.Sleep(2_000);
    }
}
