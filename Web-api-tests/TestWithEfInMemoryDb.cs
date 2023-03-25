using Forum;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;

namespace ForumTest
{
    public class TestWithEfInMemoryDb<TContext> : WebApplicationFactory<Program> where TContext : DbContext
    {
        public  HttpClient Client { get; set; }
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

                        services.AddDbContext<TContext>(options =>
                        {
                            options.UseInMemoryDatabase("InMemoryTestDb_" + Guid.NewGuid().ToString());
                        });

                        var sp = services.BuildServiceProvider();
                        using (var scope = sp.CreateScope())
                        {
                            var scopedServices = scope.ServiceProvider;
                            var db = scopedServices.GetRequiredService<TContext>();

                            // Ensure the database is created.
                            db.Database.EnsureCreated();
                        }
                    });
                });

            Client = factory.CreateClient();
        }
    }
}