using Entities;
using Entities.Models.Forum;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Microsoft.EntityFrameworkCore.Metadata.Internal;
using Newtonsoft.Json.Linq;
using System.ComponentModel;
using System.Reflection;
using System.Xml.Linq;
using Web_api_tests.Extensions;
using Xunit.Abstractions;

namespace Web_api_tests.Tests.Forum
{
    public class ForumCategoryTests : TestWithSqlite
    {
        private readonly ITestOutputHelper output;

        public ForumCategoryTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        /*public static IEnumerable<object[]> EntitiesMappedInCode =>
        typeof(TestWithSqlite).Assembly.GetTypes()
            .Where(t => t.IsSubclassOf(typeof(DbContext)) && !t.IsAbstract)
            .SelectMany(dbContextType =>
            {
                var dbSetProps = dbContextType.GetProperties()
                    .Where(c => c.PropertyType.IsGenericType
                                && typeof(DbSet<>).IsAssignableFrom(c.PropertyType.GetGenericTypeDefinition()));
                var context = Container.GetInstance(dbContextType);
                return dbSetProps.Select(dbSetProp => new[] { context, dbSetProp.GetValue(context) });
            });*/


        [Fact]
        public void TableShouldGetCreated1()
        {
            /*var contexts = typeof(ForumContext).Assembly.GetTypes()
                .Where(t => t.IsSubclassOf(typeof(DbContext)) && !t.IsAbstract);

            output.WriteLine("Contexts count: " + contexts.Count());

            foreach (var cnt in contexts.Where(c => c.Name == "ForumContext")) 
            {
                output.WriteLine("C -> " + cnt.Name);
            }

            var props = contexts.SelectMany(dbContextType =>
            {
                var dbSetProps = dbContextType.GetProperties()
                    .Where(c => c.PropertyType.IsGenericType
                                && c.Name.StartsWith("Forum")
                                && typeof(DbSet<>).IsAssignableFrom(c.PropertyType.GetGenericTypeDefinition()));

                return dbSetProps;
            });

            output.WriteLine("Props count: " + props.Count());

            foreach (var prop in props)
            {
                output.WriteLine("P -> " + prop.Name);
            }*/


            var seedCats = DbContext.GetService<IDesignTimeModel>().Model.GetPopulatedModelWithSeedDataFromConfig<ForumCategory>();

            foreach(var seedCat in seedCats) 
            {
                output.WriteLine("SC -> " + seedCat.Id + " " + seedCat.Name);
            }
        }
        
        [Fact]
        public void TableShouldGetCreated()
        {
            Assert.True(DbContext.ForumCategories.Any());
        }
        [Theory]
        [MemberData(nameof(TestCases.GetTestDataForForumCategory), MemberType = typeof(TestCases))]
        public void TableShouldContainSeedData(ForumCategory forumCategory)
        {
            var dbData = DbContext.ForumCategories
                .Where(fc => fc.Id.Equals(forumCategory.Id)
                && fc.Name.Equals(forumCategory.Name)
                && fc.ForumUserId.Equals(forumCategory.ForumUserId)).FirstOrDefaultAsync().Result;

            output.WriteLine(dbData.Name);

            dbData.Should()
                .Match<ForumCategory>((x) =>
                x.Id == forumCategory.Id 
                && x.Name == forumCategory.Name
                && x.ForumUserId == forumCategory.ForumUserId);
        }
    }
}