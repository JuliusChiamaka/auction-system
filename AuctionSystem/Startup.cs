using AuctionSystem.Domain.Settings;
using AuctionSystem.Infrastructure.Extension;
using AuctionSystem.Service;
using AspNetCoreRateLimit;
using Hangfire;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Serilog;
using System.IO;

namespace AuctionSystem
{
    public class Startup
    {
        private readonly IConfigurationRoot configRoot;
        private AppSettings AppSettings { get; set; }

        public Startup(IConfiguration configuration)
        {
            Log.Logger = new LoggerConfiguration().ReadFrom.Configuration(configuration).CreateLogger();
            Configuration = configuration;

            IConfigurationBuilder builder = new ConfigurationBuilder().SetBasePath(Directory.GetCurrentDirectory()).AddJsonFile("appsettings.json");
            configRoot = builder.Build();

            AppSettings = new AppSettings();
            Configuration.Bind(AppSettings);
        }

        public IConfiguration Configuration { get; }

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddDbContext(Configuration, configRoot);

            services.AddHttpClient();

            services.AddValidation();

            services.AddIdentityService(Configuration);

            services.AddTransientServices();

            services.AddAutoMapper();

            services.AddScopedServices(Configuration);

            services.AddCustomOptions();

            services.AddRepositoryServices(Configuration);

            services.AddSwaggerOpenAPI();

            services.AddServiceLayer(Configuration);

            services.AddHangfire(Configuration, configRoot);

            services.AddFluentControllers();

            services.AddVersion();

            services.AddCustomOptions();


        }

        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory log)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }

            

            app.ConfigureCustomExceptionMiddleware();

            log.AddSerilog();

            app.UseIpRateLimiting();

            app.UseRouting();
            app.ConfigureHangfire();
            app.UseAuthentication();
            app.UseAuthorization();
            app.ConfigureSwagger();


            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapHangfireDashboard();
            });
        }
    }
}
