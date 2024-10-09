using System.Buffers;
using System.Diagnostics;
using System.Runtime.CompilerServices;
using System.Security.Cryptography;

namespace ThreadStarvationDemo.BackgroundNoise.BackgroundServices;

public class CpuBoundedConsumer : BackgroundService
{
    private static int counter = 0;
    static readonly Stopwatch stopwatch = new();
    protected override async Task ExecuteAsync(CancellationToken stoppingToken)
    {
        stopwatch.Start();
        await foreach (var item in GetItemsAsync(stoppingToken))
        {
            LogProgress(Interlocked.Increment(ref counter));

            // fire and forget
            Task.Run(() => Thread.Sleep(500), stoppingToken);

            // Simulate CPU-bound work
            //Parallel.For(0, 1_000, i =>
            //{
                
            //    //byte[] buffer = null;
            //    //try
            //    //{
            //    //    buffer = ArrayPool<byte>.Shared.Rent(100_000);
            //    //    Random.Shared.NextBytes(buffer.AsSpan());
            //    //    SHA256.HashData(buffer.AsSpan());
            //    //}
            //    //finally
            //    //{
            //    //    if (buffer != null)
            //    //    {
            //    //        ArrayPool<byte>.Shared.Return(buffer, clearArray: true);
            //    //    }
            //    //}
            //});
        }
    }

    private static async IAsyncEnumerable<int> GetItemsAsync([EnumeratorCancellation] CancellationToken stoppingToken)
    {
        for (var i = 0; i < 1_000_000; i++)
        {
            await Task.Delay(Random.Shared.Next(20));

            if (stoppingToken.IsCancellationRequested)
            {
                yield break;
            }

            yield return i;
        }
    }

    static void LogProgress(int c)
    {
        if (c % 500 == 0)
        {
            Console.WriteLine($"Progress: {c,6}, Time: {stopwatch.Elapsed}, OPS: {500.0 / stopwatch.Elapsed.TotalSeconds:####.##}");
            stopwatch.Restart();
        }
    }
}
