using Contracts;
using Entities;
using Entities.Models;
using LoggerService;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc.Formatters;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Repository;
using Microsoft.AspNetCore.Mvc.Versioning;
using Marvin.Cache.Headers;
using AspNetCoreRateLimit;

namespace Forum.Extensions
{
    public static class ServiceExtensions
    {
        public static void ConfigureCors(this IServiceCollection services)
        {
            services.AddCors(options =>
            {
                options.AddPolicy("CorsPolicy", builder =>
                builder.AllowAnyOrigin()
                .AllowAnyMethod()
                .AllowAnyHeader());
            });
        }
        public static void ConfigureIISIntegration(this IServiceCollection services)
        {
            services.Configure<IISOptions>(options => {});
        }
        public static void ConfigureLoggerService(this IServiceCollection services)
        {
            services.AddScoped<ILoggerManager, LoggerManager>();
        }
        public static void ConfigureSqlContext(this IServiceCollection services, IConfiguration configuration)
        {
            services.AddDbContext<ForumContext>(opts => 
            opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
            b => b.MigrationsAssembly("Forum")
            ));
            services.AddDbContext<PrinterContext>(opts =>
            opts.UseSqlServer(configuration.GetConnectionString("sqlConnection"),
            b => b.MigrationsAssembly("Forum")
            ));
        }
        public static void ConfigureRepositoryManager(this IServiceCollection services)
        {
            services.AddScoped<IRepositoryManager, RepositoryManager>();
        }
        public static void AddCustomMediaTypes(this IServiceCollection services)
        {
            services.Configure<MvcOptions>(config =>
            {
                var newtonsoftJsonOutputFormatter = config.OutputFormatters
                .OfType<NewtonsoftJsonOutputFormatter>()?.FirstOrDefault();
                if (newtonsoftJsonOutputFormatter != null)
                {
                    newtonsoftJsonOutputFormatter.SupportedMediaTypes.Add("application/sd.k.hateoas+json");
                    newtonsoftJsonOutputFormatter.SupportedMediaTypes.Add("application/sd.k.apiroot+json");
                }
                var xmlOutputFormatter = config.OutputFormatters
               .OfType<XmlDataContractSerializerOutputFormatter>()?.FirstOrDefault();
                if (xmlOutputFormatter != null)
                {
                    xmlOutputFormatter.SupportedMediaTypes.Add("application/sd.k.hateoas+xml");
                    xmlOutputFormatter.SupportedMediaTypes.Add("application/sd.k.apiroot+xml");
                }
            });
        }
        public static void ConfigureVersioning(this IServiceCollection services)
        {
            services.AddApiVersioning(opt =>
            {
                opt.ReportApiVersions = true;
                opt.AssumeDefaultVersionWhenUnspecified = true;
                opt.DefaultApiVersion = new ApiVersion(1, 0);
                opt.ApiVersionReader = new HeaderApiVersionReader("api-version");
            });
        }
        public static void ConfigureResponseCaching(this IServiceCollection services)
        {
            services.AddResponseCaching();
        }
        public static void ConfigureHttpCacheHeaders(this IServiceCollection services)
        {
            services.AddHttpCacheHeaders(
            (expirationOpt) =>
            {
                expirationOpt.MaxAge = 65;
                expirationOpt.CacheLocation = CacheLocation.Private;
            },
            (validationOpt) =>
            {
                validationOpt.MustRevalidate = true;
            });
        }
        public static void ConfigureRateLimitingOptions(this IServiceCollection services)
        {
            var rateLimitRules = new List<RateLimitRule>
            {
                new RateLimitRule
                {
                    Endpoint = "*",
                    Limit= 100,
                    Period = "5m"
                }
            };

            services.Configure<IpRateLimitOptions>(opt =>
            {
                opt.GeneralRules = rateLimitRules;
            });
            services.AddSingleton<IRateLimitCounterStore, MemoryCacheRateLimitCounterStore>();
            services.AddSingleton<IIpPolicyStore, MemoryCacheIpPolicyStore>();
            services.AddSingleton<IRateLimitConfiguration, RateLimitConfiguration>();
        }
        public static void ConfigureIdentity(this IServiceCollection services)
        {
            var builder = services.AddIdentityCore<User>(o =>
            {
                o.Password.RequireDigit = true;
                o.Password.RequireLowercase = false;
                o.Password.RequireUppercase = false;
                o.Password.RequireNonAlphanumeric = false;
                o.Password.RequiredLength = 10;
                o.User.RequireUniqueEmail = true;
            });

            builder = new IdentityBuilder(builder.UserType, typeof(IdentityRole), builder.Services);
            builder.AddRoles<IdentityRole>();
            builder.AddEntityFrameworkStores<ForumContext>()
            .AddDefaultTokenProviders();
        }
    }
}