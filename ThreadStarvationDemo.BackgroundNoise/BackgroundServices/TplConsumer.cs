using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ThreadStarvationDemo.BackgroundNoise.BackgroundServices;

public class TplConsumer : BackgroundService
{
    private static int _counter = 0;
    private static readonly Stopwatch Stopwatch = new();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        Stopwatch.Start();
        await foreach (int item in GetItemsAsync(stoppingToken))
        {
            LogProgress(Interlocked.Increment(ref _counter));

            // Simulate parallel CPU-bounded work
            Parallel.For(0, 100, i =>
            {
                Thread.Sleep(20);

                //byte[] buffer = null;
                //try
                //{
                //    buffer = ArrayPool<byte>.Shared.Rent(100_000);
                //    Random.Shared.NextBytes(buffer.AsSpan());
                //    SHA256.HashData(buffer.AsSpan());
                //}
                //finally
                //{
                //    if (buffer != null)
                //    {
                //        ArrayPool<byte>.Shared.Return(buffer, clearArray: true);
                //    }
                //}
            });
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
}
