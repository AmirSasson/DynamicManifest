using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace SomeApi
{
    public class Program
    {
        public static async  Task Main(string[] args)
        {
            var app = CreateHostBuilder(args).Build();

            // Resolve the StartupTasks from the ServiceProvider
            //var startupTask = app.Services.GetService<IServiceDiscovery>();

            //app.
            //await startupTask.Register();

            app.Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args) =>
            Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.UseStartup<Startup>();
                })            
            ;
    }
}
