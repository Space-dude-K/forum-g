﻿using AspNetCoreRateLimit;
using Interfaces;
using Interfaces.Forum;
using Entities.DTO.ForumDto;
using Entities.Models.Forum;
using Forum.ActionsFilters;
using Forum.ActionsFilters.Forum;
using Forum.ActionsFilters.User;
using Forum.Extensions;
using Forum.Utility.ForumLinks;
using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.AspNetCore.HttpOverrides;
using Microsoft.AspNetCore.Mvc;
using NLog;
using Repository.DataShaping;
using Services;
using Interfaces.User;

namespace Forum
{
    public class Startup
    {
        private readonly IConfiguration Configuration;
        private readonly string appUrl;

        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
            appUrl = configuration["ASPNETCORE_URLS"].Split(";").First();
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureLoggerService();
            services.ConfigureSqlContext(Configuration);

            services.AddAutoMapper(typeof(Startup));
            services.ConfigureRepositoryManager();
            services.AddControllers(config =>
            {
                config.RespectBrowserAcceptHeader = true;
                config.ReturnHttpNotAcceptable = true;
                config.CacheProfiles.Add("60SecondsDuration", new CacheProfile
                {
                    Duration = 60
                });
            })
                .AddNewtonsoftJson()
                .AddXmlDataContractSerializerFormatters();

            // To return 422 instead of 400, the first thing we have to do is to suppress
            // the BadRequest error when the ModelState is invalid
            services.Configure<ApiBehaviorOptions>(options =>
            {
                options.SuppressModelStateInvalidFilter = true;
            });

            services.AddScoped<ValidateRoleExistsAttribute>();

            services.AddScoped<ValidationFilterAttribute>();
            services.AddScoped<ValidateCategoryExistsAttribute>();
            services.AddScoped<ValidateForumForCategoryExistsAttribute>();
            services.AddScoped<ValidateTopicForForumExistsAttribute>();
            services.AddScoped<ValidatePostForTopicExistsAttribute>();

            services.AddScoped<ValidateMediaTypeAttribute>();

            services.AddScoped<IDataShaper<ForumCategoryDto>, DataShaper<ForumCategoryDto>>();
            services.AddScoped<IDataShaper<ForumBaseDto>, DataShaper<ForumBaseDto>>();
            services.AddScoped<IDataShaper<ForumTopicDto>, DataShaper<ForumTopicDto>>();
            services.AddScoped<IDataShaper<ForumPostDto>, DataShaper<ForumPostDto>>();

            System.Diagnostics.Debug.WriteLine("TEST: " + appUrl);

            services.AddHttpClient<IAuthenticationService, AuthenticationService>(c =>
                c.BaseAddress = new Uri(appUrl));
            services.AddHttpClient<IUserService, UserService>(c =>
                c.BaseAddress = new Uri(appUrl));

            services.AddCustomMediaTypes();

            // HATEOAS
            services.AddScoped<CategoryLinks>();
            services.AddScoped<ForumBaseLinks>();
            services.AddScoped<TopicLinks>();
            services.AddScoped<PostLinks>();

            // Versioning service
            services.ConfigureVersioning();

            // Caching
            services.ConfigureResponseCaching();
            services.ConfigureHttpCacheHeaders();
            services.AddHttpContextAccessor();

            // Memory cache for memory cache library
            services.AddMemoryCache();

            // Rate limiting, throttling
            services.ConfigureRateLimitingOptions();
            services.AddHttpContextAccessor();
            services.AddSingleton<IProcessingStrategy, AsyncKeyLockProcessingStrategy>();

            // Authentication and autorization
            services.AddAuthentication();
            services.ConfigureIdentity();
            services.ConfigureJWT(Configuration);
            services.AddScoped<IAuthenticationManager, AuthenticationManager>();

            services.AddControllersWithViews();
        }
        public void Configure(IApplicationBuilder app, IWebHostEnvironment env, ILoggerManager logger)
        {
            if (env.IsDevelopment())
            {
                app.UseDeveloperExceptionPage();
            }
            else
            {
                // will add middleware for using HSTS, which adds the 
                // Strict - Transport - Security header
                app.UseHsts();
                app.UseExceptionHandler("/Home/Error");
            }

            app.ConfigureExceptionHandler(logger);
            app.UseHttpsRedirection();
            // enables using static files for the request. If we don’t set a path to the static files directory, it will use a wwwroot
            // folder in our project by default.
            //app.UseDefaultFiles();
            app.UseStaticFiles();

            app.UseHttpsRedirection();

            app.UseCors("CorsPolicy");

            // will forward proxy headers to the current request. This will help us during application deployment
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });

            app.UseResponseCaching();
            app.UseHttpCacheHeaders();

            app.UseIpRateLimiting();

            app.UseRouting();

            app.UseAuthentication();
            app.UseAuthorization();

            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
                endpoints.MapControllerRoute(
                    name: "default",
                    pattern: "{controller=Home}/{action=Index}/{id?}");
            });
        }
    }
}