using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Hosting;
using System;
using System.Diagnostics;

namespace ThreadStarvationDemo.CoreWebAPI
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.WriteLine("PID: {0}", Environment.ProcessId);
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                });
    }
}
