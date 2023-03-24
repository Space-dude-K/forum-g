using Entities.Models.Forum;
using FluentAssertions;
using Forum;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using System.Net;
using Web_api_tests.Tests.Forum.TestCases;
using Xunit.Abstractions;

namespace Web_api_tests.Tests.Forum
{
    public class ForumCategoryTest : TestWithSqlite, IClassFixture<WebApplicationFactory<Program>>
    {
        private readonly ITestOutputHelper output;
        private readonly WebApplicationFactory<Program> factory;

        public ForumCategoryTest(ITestOutputHelper output, WebApplicationFactory<Program> factory)
        {
            this.output = output;
            this.factory = factory;
        }
        [Fact]
        public void TableShouldGetCreated()
        {
            Assert.True(DbContext.ForumCategories.Any());
        }
        [Theory, ClassData(typeof(TestClassForForumCategoryData))]
        public void TableShouldContainSeedData<T>(ForumCategory forumCategory)
        {
            var dbData = DbContext.ForumCategories
                .Where(fc => fc.Id.Equals(forumCategory.Id)
                && fc.Name.Equals(forumCategory.Name)
                && fc.ForumUserId.Equals(forumCategory.ForumUserId)).FirstOrDefaultAsync().Result;

            dbData.Should()
                .Match<ForumCategory>((x) =>
                x.Id == forumCategory.Id
                && x.Name == forumCategory.Name
                && x.ForumUserId == forumCategory.ForumUserId);
        }
        [Theory]
        [InlineData("/api/categories")]
        public async Task GetAll_ForumCategories_ReturnsCorrectContentType(string url)
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
        [Theory]
        [InlineData("/api/categories/2")]
        public async Task GetSingle_ForumCategory_ReturnsTestData(string url)
        {
            // Arrange
            var client = factory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal("application/json; charset=utf-8",
                response.Content.Headers.ContentType.ToString());
        }
        [Fact]
        public void InsertTest_ForumCategoriesData_ReturnsTestCaseData()
        {


            Assert.True(DbContext.ForumCategories.Any());
        }
    }
}