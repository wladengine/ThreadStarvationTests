﻿using System.Diagnostics;
using System.Runtime.CompilerServices;

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
            Task.Factory.StartNew((i) => HandleAndSaveResults((int)i), item, TaskCreationOptions.LongRunning);
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

    static void HandleAndSaveResults(int item)
    {
        // Simulate CPU-bound work
        Thread.Sleep(2_000);
    }
}
