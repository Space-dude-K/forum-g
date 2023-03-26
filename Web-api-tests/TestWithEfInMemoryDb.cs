using Entities;
using Entities.Models.Forum;
using Forum;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.VisualStudio.TestPlatform.Utilities;
using System.Diagnostics;
using Xunit.Abstractions;

namespace ForumTest
{
    public class TestWithEfInMemoryDb<TContext> : WebApplicationFactory<Startup> where TContext : DbContext
    {
        public HttpClient HttpClient { get; set; }
        public TContext Context { get; set; }
        public IModel Model { get; set; }
        public TestWithEfInMemoryDb()
        {
            HttpClient = this.CreateClient();
        }
        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            builder.ConfigureTestServices(services =>
            {
                // remove the existing context configuration
                var descriptor = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<TContext>));
                if (descriptor != null)
                    services.Remove(descriptor);

                var dbName = "InMemoryTestDb_" + Guid.NewGuid().ToString();
                services.AddDbContext<TContext>(options =>
                    options.UseInMemoryDatabase(dbName));
                Debug.WriteLine("Db name: " + dbName);

                var sp = services.BuildServiceProvider();
                using (var scope = sp.CreateScope())
                {
                    var scopedServices = scope.ServiceProvider;
                    Context = scopedServices.GetRequiredService<TContext>();

                    Model = Context.GetService<IDesignTimeModel>().Model;

                    // Ensure the database is created.
                    Context.Database.EnsureDeleted();
                    Context.Database.EnsureCreated();
                }
            });
        }
        /*public TestWithEfInMemoryDb()
        {
            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(host =>
                {
                    host.ConfigureTestServices(services =>
                    {
                        var descriptor = services.SingleOrDefault(
                            d => d.ServiceType == typeof(DbContextOptions<TContext>));
                        services.Remove(descriptor);
 
                        var dbName = "InMemoryTestDb_" + Guid.NewGuid().ToString();

                        services.AddDbContext<TContext>(options =>
                        {
                            options.UseInMemoryDatabase(dbName);
                        });

                        var sp = services.BuildServiceProvider();
                        using (var scope = sp.CreateScope())
                        {
                            var scopedServices = scope.ServiceProvider;
                            Context = scopedServices.GetRequiredService<TContext>();

                            Model = Context.GetService<IDesignTimeModel>().Model;

                            // Ensure the database is created.
                            Context.Database.EnsureCreated();

                            //ConnString = Context.Database.GetConnectionString();
                        }
                    });
                });

            Client = factory.CreateClient();
        }*/
    }
}