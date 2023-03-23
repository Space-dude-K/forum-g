using Entities.Models.Forum;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Xunit.Abstractions;

namespace Web_api_tests.Tests.Forum
{
    public class ForumBaseTests : TestWithSqlite
    {
        private readonly ITestOutputHelper output;

        public ForumBaseTests(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public void TableShouldGetCreated()
        {
            Assert.True(DbContext.ForumBases.Any());
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
