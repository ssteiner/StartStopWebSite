using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.OpenApi.Models;
using System;

namespace StartopStopWebApplication2
{
    public class Startup
    {

        private ILogger logger;
        private readonly Guid instance;

        public Startup(IConfiguration configuration)
        {
            Configuration = configuration;
            instance = Guid.NewGuid();
        }

        public Startup(IConfiguration configuration, ILogger logger)
            : this(configuration)
        {
            this.logger = logger;
            logger.LogCritical($"Startup created, instance {instance}");
        }

        public IConfiguration Configuration { get; }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            logger?.LogCritical("ConfigureServices called, instance {instance}");
            services.AddControllers();
            services.AddSwaggerGen(c =>
            {
                c.SwaggerDoc("v1", new OpenApiInfo { Title = "StartopStopWebApplication2", Version = "v1" });
            });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, IHostApplicationLifetime hostApplicationLifetime, ILogger<Startup> logger)
        {
            if (this.logger == null)
                this.logger = logger;
            logger.LogInformation($"{nameof(Configure)} called in instance {instance}, envornment name {env.EnvironmentName}, app name {env.ApplicationName}");
            hostApplicationLifetime.ApplicationStopping.Register(OnStop);
            if (env.IsDevelopment())
            {
                logger.LogInformation($"Application is running in development mode");
                app.UseDeveloperExceptionPage();
                app.UseSwagger();
                app.UseSwaggerUI(c => c.SwaggerEndpoint("/swagger/v1/swagger.json", "StartopStopWebApplication2 v1"));
            }

            app.UseHttpsRedirection();

            app.UseRouting();

            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }

        private void OnStop()
        {
            logger?.LogCritical($"Application instance {instance} stopping");
        }

        private void OnStart()
        {
            logger?.LogCritical($"Application instance {instance} stopping");
        }
    }
}
