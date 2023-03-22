using Entities.Models.Forum;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
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