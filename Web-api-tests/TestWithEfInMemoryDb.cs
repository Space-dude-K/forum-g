using Entities;
using Entities.Models.Forum;
using Forum;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.Extensions.DependencyInjection;

namespace ForumTest
{
    public class TestWithEfInMemoryDb<TContext> : WebApplicationFactory<Program> where TContext : DbContext
    {
        public  HttpClient Client { get; set; }
        public TContext Context { get; set; }
        public IModel Model { get; set; }
        public TestWithEfInMemoryDb()
        {
            var factory = new WebApplicationFactory<Program>()
                .WithWebHostBuilder(host =>
                {
                    host.ConfigureServices(services =>
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
                        }
                    });
                });

            Client = factory.CreateClient();
        }
    }
}