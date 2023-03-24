using Entities.Models.Forum;
using FluentAssertions;
using Microsoft.EntityFrameworkCore;
using Web_api_tests.Tests.Forum.TestCases;
using Xunit.Abstractions;

namespace Web_api_tests.Tests.Forum
{
    public class ForumPostTest : TestWithSqlite
    {
        private readonly ITestOutputHelper output;

        public ForumPostTest(ITestOutputHelper output)
        {
            this.output = output;
        }
        [Fact]
        public void TableShouldGetCreated()
        {
            Assert.True(DbContext.ForumPosts.Any());
        }
        [Theory, ClassData(typeof(TestClassForForumPostData))]
        public void TableShouldContainSeedData<T>(ForumPost forumPost)
        {
            var dbData = DbContext.ForumPosts
                .Where(fc => fc.Id.Equals(forumPost.Id)
                && fc.PostName.Equals(forumPost.PostName)
                && fc.ForumTopicId.Equals(forumPost.ForumTopicId)
                ).FirstOrDefaultAsync().Result;

            dbData.Should()
                .Match<ForumPost>((x) =>
                x.Id == forumPost.Id
                && x.PostName == forumPost.PostName
                && x.ForumTopicId == forumPost.ForumTopicId
                && x.ForumUserId == forumPost.ForumUserId);
        }
    }
}