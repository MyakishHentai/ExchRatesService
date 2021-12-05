using ExchRates.Common.Caching;
using ExchRatesFrontService.Config;
using ExchRatesFrontService.Services;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.OpenApi.Models;
using System;
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
                .AddHttpContextAccessor()
                .AddSwaggerGen(opt =>
                {
                    opt.SwaggerDoc("v1", new OpenApiInfo { Title = $"{Program.ApplicationName} API", Version = "v1" });
                    opt.CustomSchemaIds(type => type.FullName);
                })
                .Configure<ServiceConfig>(_configuration)
                .AddControllers()
                .AddJsonOptions(opt =>
                {
                    opt.JsonSerializerOptions.PropertyNamingPolicy = JsonNamingPolicy.CamelCase;
                    opt.JsonSerializerOptions.Encoder =
                        JavaScriptEncoder.Create(UnicodeRanges.BasicLatin, UnicodeRanges.Cyrillic);
                    opt.JsonSerializerOptions.IgnoreNullValues = true;
                });

            services.AddGrpcClient<ExchRatesSvc.ExchRates.ExchRatesClient>
                (opt => opt.Address = new Uri("https://localhost:5001"));         

            if (!string.IsNullOrWhiteSpace(serviceConfig.GAIA_ADDR))
            {
                // TODO: cache
                // Добавить основной сервис
            }
            else
            {
                // TODO: cache
                // Добавить вторичный сервис
            }
            services
                .AddMemoryCache()
                .AddScoped<IMemoryCacheService, MemoryCacheService>()
                .AddScoped<IExchRatesService, ExchRatesService>();
        }

        // This method gets called by the runtime. Use this method to configure the HTTP request pipeline.
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env)
        {
            ServiceConfig serviceConfig = _configuration.Get<ServiceConfig>();

            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            app
                .UseSwagger()
                .UseSwaggerUI(opt =>
                    opt.SwaggerEndpoint("/swagger/v1/swagger.json",
                    $"{Program.ApplicationName} v1"));
            if (!string.IsNullOrWhiteSpace(serviceConfig.GAIA_ADDR))
            {
                // TODO: cache
            }
            else
            {
                // TODO: cache
                // app.UseMiddleware<>();
            }

            app
                .UseHttpsRedirection()
                .UseRouting()
                .UseAuthorization()
                .UseEndpoints(endpoints => endpoints.MapControllers());
        }
    }
}
