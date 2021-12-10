using ExchRates.Common.Caching;
using ExchRates.Common.Caching.Interfaces;
using ExchRates.Common.Extensions;
using ExchRates.Common.Middleware;
using ExchRates.Common.Repositories;
using ExchRates.Common.Services;
using ExchRatesFrontService.Config;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.IdentityModel.Tokens;
using Microsoft.OpenApi.Models;
using System;
using System.IO;
using System.Text;
using System.Text.Encodings.Web;
using System.Text.Json;
using System.Text.Unicode;

namespace ExchRatesFrontService
{
    public class Startup
    {
        private readonly IConfiguration _configuration;

        public Startup(IConfiguration configuration)
        {
            _configuration = configuration;
        }

        // This method gets called by the runtime. Use this method to add services to the container.
        public void ConfigureServices(IServiceCollection services)
        {
            ServiceConfig serviceConfig = _configuration.Get<ServiceConfig>();

            if (serviceConfig.IsSecure)
            {
                AppContext.SetSwitch("System.Net.Http.SocketsHttpHandler.Http2UnencryptedSupport", true);
            }

            services
                .AddGrpcClient<ExchRatesSvc.ExchRates.ExchRatesClient>
                (opt => opt.Address = new Uri(serviceConfig.BackAddress));

            services
                .AddHttpContextAccessor()
                .AddSwaggerGen(opt =>
                {
                    opt.SwaggerDoc("v1", new OpenApiInfo { Title = $"{Program.ApplicationName} API", Version = "v1" });
                    opt.CustomSchemaIds(type => type.FullName);
                })
                .Configure<ServiceConfig>(_configuration)
                .AddScoped<LoggingMiddleware>()
                .AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    opt.JsonSerializerOptions.Encoder =
                        JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);
                    opt.JsonSerializerOptions.IgnoreNullValues = true;
                });
            // Локальное
            if (serviceConfig.IsMemoryCache)
            {
                services
                    .AddMemoryCache()
                    .AddScoped<ICacheService, MemoryCacheService>();
            }
            // Распределенное кэширование
            else
            {
                services
                    .AddStackExchangeRedisCache(opt =>
                    {
                        opt.Configuration = serviceConfig.CacheAddress;
                    })
                    .AddScoped<ICacheService, DistributedCacheService>();
            }
            // JWT аутентификация Middleware.
            services
                .AddDistributedMemoryCache()
                .AddSession(options =>
                {
                    options.IdleTimeout = TimeSpan.FromMinutes(30);
                    options.Cookie.HttpOnly = true;
                    options.Cookie.IsEssential = true;
                })
                .AddTransient<IUserRepository, UserRepository>()
                .AddTransient<ITokenService, TokenService>()
                .AddAuthorization()
                .AddAuthentication(JwtBearerDefaults.AuthenticationScheme).AddJwtBearer(options =>
                {
                    options.TokenValidationParameters = new TokenValidationParameters
                    {
                        ValidateIssuer = true,
                        ValidateAudience = true,
                        ValidateLifetime = true,
                        ValidateIssuerSigningKey = true,
                        ValidIssuer = "Jwt:Issuer",
                        ValidAudience = "Jwt:Issuer",
                        IssuerSigningKey = new SymmetricSecurityKey
                        (Encoding.UTF8.GetBytes("Jwt:Key"))
                    };
                });
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerFactory loggerFactory)
        {
            ServiceConfig serviceConfig = _configuration.Get<ServiceConfig>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            if (serviceConfig.IsFileLog)
            {
                loggerFactory.AddFile(Path.Combine(Directory.GetCurrentDirectory(), "Logs\\log.txt"));
            }

            app
                .UseSwagger()
                .UseSwaggerUI(opt =>
                    opt.SwaggerEndpoint("/swagger/v1/swagger.json",
                    $"{Program.ApplicationName} v1"));
            app
                .UseHttpsRedirection()
                .UseCors()
                .UseSession()
                .UseRouting()
                .UseAuthorization()
                .UseAuthentication()
                .UseMiddleware<LoggingMiddleware>()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
