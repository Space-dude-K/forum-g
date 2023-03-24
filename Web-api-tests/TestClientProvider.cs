using Forum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.TestHost;
using Entities;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using FluentAssertions.Common;
using Microsoft.AspNetCore.Hosting.Server;
using System.Configuration;
using System.Diagnostics;
using Xunit.Abstractions;

namespace Web_api_tests
{
    public class TestClientProvider : IDisposable
    {
        private TestServer server;
        private readonly ITestOutputHelper output;

        public HttpClient Client { get; private set; }

        public TestClientProvider(ITestOutputHelper output)
        {
            var builder = new WebHostBuilder().UseStartup<Startup>();
            builder.UseEnvironment("Development");
            /*builder.ConfigureTestServices(s =>
            {
                var contextDescriptor = s.FirstOrDefault(descriptor => descriptor.ServiceType == typeof(ForumContext));
                s.Remove(contextDescriptor);
                s.AddDbContext<ForumContext>(opts =>
                opts.UseSqlServer(inMemoryConnectionString, b => b.MigrationsAssembly("Forum")));
                s.AddLogging(builder => builder
                .AddDebug()
                .AddFilter(level => level >= LogLevel.Trace)
                );
            });*/

            output.WriteLine("Sql conn: " + builder.GetSetting("sqlConnection"));

            builder.ConfigureTestServices(services =>
            {
                services.AddDbContext<ForumContext>(opts =>
                 opts.UseSqlServer(builder.GetSetting("sqlConnection"),
                 b => b.MigrationsAssembly("Forum")
                 ));
            });

            server = new TestServer(builder);

            var context = server.Services.GetRequiredService<ForumContext>();
            context.Database.EnsureCreated();

            Client = server.CreateClient();
            this.output = output;
        }
        public void Dispose()
        {
            server?.Dispose();
            Client?.Dispose();
        }
    }
}