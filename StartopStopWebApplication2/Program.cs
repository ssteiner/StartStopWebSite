using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using System.IO;

namespace StartopStopWebApplication2
{
    public class Program
    {
        public static void Main(string[] args)
        {
            CreateHostBuilder(args).Build().Run();
        }

        public static IHostBuilder CreateHostBuilder(string[] args)
        {
            var config = GetConfiguration();
            return Host.CreateDefaultBuilder(args)
                .ConfigureWebHostDefaults(webBuilder =>
                {
                    webBuilder.ConfigureLogging(builder => ConfigureLogging(builder));
                    webBuilder.UseStartup(context => new Startup(config, GetLogger<Startup>()));
                });
        }

        private static ILogger<T> GetLogger<T>() where T: class
        {
            var factory = LoggerFactory.Create(builder => ConfigureLogging(builder));
            return factory.CreateLogger<T>();
        }

        private static ILoggingBuilder ConfigureLogging(ILoggingBuilder builder)
        {
            return builder.AddConsole().AddFile(options => 
            {
                options.FileName = "StartStopLogging";
                options.LogDirectory = @"c:\temp\";
                options.Extension = "log";
            });
        }

        private static IConfiguration GetConfiguration()
        {
            var folder = Path.GetDirectoryName(typeof(Program).Assembly.Location);
            string fileName = "appSettings.json";
            var typeLocation = Path.Combine(folder, fileName);
            if (!File.Exists(typeLocation))
                return null;
            var builder = new ConfigurationBuilder()
                .SetBasePath(folder)
                .AddJsonFile(fileName, true, true);
            var config = builder.Build();
            return config;
        }
    }
}
