using Contracts;
using Forum.Extensions;
using Microsoft.AspNetCore.HttpOverrides;
using NLog;
using NLog.Filters;
using System.Reflection.PortableExecutable;
using System.Security.Cryptography.Xml;

namespace Forum
{
    internal class Startup
    {
        private readonly IConfiguration Configuration;

        public Startup(IConfiguration configuration)
        {
            LogManager.LoadConfiguration(string.Concat(Directory.GetCurrentDirectory(), "/nlog.config"));
            Configuration = configuration;
        }
        public void ConfigureServices(IServiceCollection services)
        {
            services.ConfigureCors();
            services.ConfigureIISIntegration();
            services.ConfigureLoggerService();
            services.ConfigureSqlContext(Configuration);


            services.AddControllers();
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
            }
            app.UseHttpsRedirection();
            // enables using static files for the request. If we don’t set a path to the static files directory, it will use a wwwroot
            // folder in our project by default.
            app.UseStaticFiles();
            app.UseCors("CorsPolicy");

            // will forward proxy headers to the current request. This will help us during application deployment
            app.UseForwardedHeaders(new ForwardedHeadersOptions
            {
                ForwardedHeaders = ForwardedHeaders.All
            });
            app.UseRouting();
            // app.UseAuthorization();
            app.UseEndpoints(endpoints =>
            {
                endpoints.MapControllers();
            });
        }
    }
}