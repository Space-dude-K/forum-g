using Entities;
using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.Create;
using Forum;
using ForumTest.Tests.Integration.Forum.TestCases;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;
using System.Net;
using Xunit.Abstractions;

namespace ForumTest.Tests.Integration.Forum
{
    public class ForumCategoryWebApiTest : IClassFixture<TestWithEfInMemoryDb<ForumContext>>
    {
        private readonly ITestOutputHelper _output;
        private readonly TestWithEfInMemoryDb<ForumContext> _webApplicationFactory;

        public ForumCategoryWebApiTest(ITestOutputHelper output, TestWithEfInMemoryDb<ForumContext> webApplicationFactory)
        {
            _output = output;
            _webApplicationFactory = webApplicationFactory;
        }

        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetAllCategoriesData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetAll_ForumCategories_ReturnsCorrectContentType(string url, string contentType)
        {
            _output.WriteLine("Data -> " + url + " Type: " + contentType);

            // Arrange
            var client = _webApplicationFactory.Client;

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var rawData = await response.Content.ReadAsStringAsync();
            _output.WriteLine("Raw json: " + rawData);
            var content = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawData);

            _output.WriteLine("Parsed json: " + content.First().Id + " " + content.First().Name);

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(contentType, response.Content.Headers.ContentType.ToString());
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetSingleForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetSingle_ForumCategory_ReturnsCorrectContentType_ReturnsTestData(string url, string contentType)
        {
            _output.WriteLine("Data -> " + url);

            // Arrange
            var client = _webApplicationFactory.CreateClient();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(contentType, response.Content.Headers.ContentType.ToString());
        }
    }
}
