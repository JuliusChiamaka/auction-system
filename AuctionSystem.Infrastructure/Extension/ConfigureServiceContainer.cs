using AuctionSystem.Infrastructure.Configs;
using AuctionSystem.Persistence;
using AuctionSystem.Service.Contract;
using AuctionSystem.Service.Contract.Repository;
using AuctionSystem.Service.Implementation;
using AuctionSystem.Service.Repository;
using AspNetCoreRateLimit;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using Hangfire;
using Hangfire.SqlServer;
using AuctionSystem.Domain.Utils;
using AuctionSystem.Domain.Settings;

namespace AuctionSystem.Infrastructure.Extension
{
    public static class ConfigureServiceContainer
    {
        public static void AddDbContext(this IServiceCollection serviceCollection,
             IConfiguration configuration, IConfigurationRoot configRoot)
        {
            string connectionString = configuration.GetConnectionString("DBConnectionString") ?? configRoot["ConnectionStrings:DBConnectionString"];
            serviceCollection.AddDbContext<ApplicationDbContext>(options =>
                   options.UseSqlServer(connectionString, b =>
                   {
                       b.MigrationsAssembly(typeof(ApplicationDbContext).Assembly.FullName);
                       b.EnableRetryOnFailure(5, TimeSpan.FromSeconds(10), null);
                   }));
        }

        public static void AddHangfire(this IServiceCollection serviceCollection,
            IConfiguration config, IConfigurationRoot configRoot)
        {
            var hangfireDbContext = serviceCollection.BuildServiceProvider().GetService<IApplicationDbContext>();

            serviceCollection.AddHangfire(configuration => configuration
          .SetDataCompatibilityLevel(CompatibilityLevel.Version_170)
          .UseSimpleAssemblyNameTypeSerializer()
          .UseRecommendedSerializerSettings()
          .UseSqlServerStorage(config.GetConnectionString("DBConnectionString") ?? configRoot["ConnectionStrings:DBConnectionString"], new SqlServerStorageOptions
          {
              CommandBatchMaxTimeout = TimeSpan.FromMinutes(5),
              SlidingInvisibilityTimeout = TimeSpan.FromMinutes(5),
              QueuePollInterval = TimeSpan.FromSeconds(15),
              SchemaName = "Test Auction System",
              UseRecommendedIsolationLevel = true,
              UsePageLocksOnDequeue = true,
              DisableGlobalLocks = true,
          }));

            serviceCollection.AddHangfireServer(options =>
            {
                options.ServerName = String.Format("{0}.{1}", Environment.MachineName, Guid.NewGuid().ToString());
                options.WorkerCount = 1;
                options.Queues = new[] {
                      HangfireQueues.Default,
                    };
            });

            serviceCollection.AddHangfireServer(options =>
            {
                options.ServerName = String.Format("{0}.{1}", Environment.MachineName, HangfireQueues.ReminderEmails.ToString());
                options.WorkerCount = 1;
                options.Queues = new[] {
                      HangfireQueues.ReminderEmails,
                    };
            });
        }
        public static void AddTransientServices(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddTransient<IClientFactory, ClientFactory>();
            serviceCollection.AddTransient<INotificationService, NotificationService>();
            serviceCollection.AddTransient<ITemplateService, TemplateService>();

        }

        public static void AddAutoMapper(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddAutoMapper(typeof(MappingProfileConfiguration));
        }

        public static void AddFluentControllers(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddControllersWithViews()
                .AddNewtonsoftJson(ops => { ops.SerializerSettings.NullValueHandling = Newtonsoft.Json.NullValueHandling.Ignore; });
            serviceCollection.AddRazorPages();

            serviceCollection.Configure<ApiBehaviorOptions>(apiBehaviorOptions =>
                apiBehaviorOptions.InvalidModelStateResponseFactory = actionContext =>
                {
                    return new BadRequestObjectResult(new
                    {
                        Succeeded = false,
                        Code = 400,
                        Message = "Validation Error",
                        Errors = actionContext.ModelState.Values.SelectMany(x => x.Errors)
                        .Select(x => x.ErrorMessage)
                    });
                });
        }

        public static void AddScopedServices(this IServiceCollection serviceCollection, IConfiguration config)
        {
            serviceCollection.AddScoped<IApplicationDbContext>(provider => provider.GetService<ApplicationDbContext>());
            serviceCollection.AddScoped<IAccountService, AccountService>();
            serviceCollection.AddScoped<IPendingUserService, PendingUserService>();
        }

        public static void AddCustomOptions(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddOptions<APISettingsOptions>().BindConfiguration("APISettings");

        }
        public static void AddRepositoryServices(this IServiceCollection serviceCollection, IConfiguration config)
        {
            serviceCollection.AddSingleton<IUserRepository, UserRepository>();
            serviceCollection.AddSingleton<IPendingUserRepository, PendingUserRepository>();
        }

        public static void AddValidation(this IServiceCollection serviceCollection)
        {
            //Disable Automatic Model State Validation built-in to ASP.NET Core
            serviceCollection.Configure<ApiBehaviorOptions>(opt => { opt.SuppressModelStateInvalidFilter = false; });
        }

        public static void AddSwaggerOpenAPI(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddSwaggerGen(setupAction =>
            {

                setupAction.SwaggerDoc(
                    "OpenAPISpecification",
                    new OpenApiInfo()
                    {
                        Title = "Test Auction System",
                        Version = "1",
                        Description = "Portal to manage the Test Auction System."
                    });

                setupAction.AddSecurityDefinition("Bearer", new OpenApiSecurityScheme
                {
                    Type = SecuritySchemeType.Http,
                    Scheme = "bearer",
                    BearerFormat = "JWT",
                    Description = $"Input your Bearer token in this format - Bearer token to access this API",
                });
                setupAction.AddSecurityRequirement(new OpenApiSecurityRequirement
                {
                    {
                        new OpenApiSecurityScheme
                        {
                            Reference = new OpenApiReference
                            {
                                Type = ReferenceType.SecurityScheme,
                                Id = "Bearer",
                            },
                        }, new List<string>()
                    },
                });
            });

        }

        public static void AddVersion(this IServiceCollection serviceCollection)
        {
            serviceCollection.AddApiVersioning(config =>
            {
                config.DefaultApiVersion = new ApiVersion(1, 0);
                config.AssumeDefaultVersionWhenUnspecified = true;
                config.ReportApiVersions = true;
            });
        }

    }
}
