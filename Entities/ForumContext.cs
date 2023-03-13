using Entities.Configuration;
using Entities.Models;
using Microsoft.AspNetCore.Identity.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore;

namespace Entities
{
    public class ForumContext : IdentityDbContext<ApplicationUser>
    {
        public ForumContext(DbContextOptions options) : base(options)
        {
        }
        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            base.OnModelCreating(modelBuilder);

            modelBuilder.ApplyConfiguration(new AccountConfiguration());
            modelBuilder.ApplyConfiguration(new ForumAccountTypeConfiguration());
            modelBuilder.ApplyConfiguration(new ForumBaseConfiguration());
            modelBuilder.ApplyConfiguration(new ForumCategoryConfiguration());
            modelBuilder.ApplyConfiguration(new ForumPostConfiguration());
            modelBuilder.ApplyConfiguration(new ForumTopicConfiguration());
            modelBuilder.ApplyConfiguration(new ForumUserConfiguration());
        }
        public DbSet<ForumUser> ForumUsers { get; set; }
        public DbSet<ForumCategory> ForumCategories { get; set; }
        public DbSet<ForumBase> ForumBases { get; set; }
        public DbSet<ForumTopic> ForumTopics { get; set; }
        public DbSet<ForumPost> ForumPosts { get; set; }
        public DbSet<ForumAccountType> ForumAccountTypes { get; set; }
    }
}