using System.Diagnostics;
using System.Runtime.CompilerServices;

namespace ThreadStarvationDemo.BackgroundNoise.BackgroundServices;

public class TplConsumer : BackgroundService
{
    private static int counter = 0;
    static readonly Stopwatch stopwatch = new();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stopwatch.Start();
        await foreach (var item in GetItemsAsync(stoppingToken))
        {
            LogProgress(Interlocked.Increment(ref counter));

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

    static void LogProgress(int c)
    {
        if (c % 100 == 0)
        {
            Console.WriteLine($"Progress: {c,6}, Time: {stopwatch.Elapsed}, OPS: {100.0 / stopwatch.Elapsed.TotalSeconds:####.##}");
            stopwatch.Restart();
        }
    }
}
