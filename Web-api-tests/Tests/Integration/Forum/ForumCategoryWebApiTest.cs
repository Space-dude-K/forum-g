using Entities;
using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.Create;
using Entities.Models.Forum;
using Forum;
using ForumTest.Extensions;
using ForumTest.Tests.Integration.Forum.TestCases;
using ForumTest.Tests.Unit.Forum.TestCases;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.EntityFrameworkCore;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using Newtonsoft.Json;
using System.Net;
using System.Text;
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

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(contentType, response.Content.Headers.ContentType.ToString());
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetAllCategoriesData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetAll_ForumCategories_ReturnsSeedData(string url, string contentType)
        {
            _output.WriteLine("Data -> " + url + " Type: " + contentType);

            // Arrange
            var client = _webApplicationFactory.Client;
            var seedData = _webApplicationFactory.Model.GetPopulatedModelWithSeedDataFromConfig<ForumCategory>();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var rawData = await response.Content.ReadAsStringAsync();
            var content = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawData);

            foreach(var fcDb in content)
            {
                var res = seedData.Any(fc => fc.Id.Equals(fcDb.Id) && fc.Name.Equals(fcDb.Name));
                Assert.True(res, $"Db data with Id: {fcDb.Id} are not equal to seed data");
            }
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetSingleForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetSingle_ForumCategory_ReturnsCorrectContentType(string url, string contentType)
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
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetSingleForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetSingle_ForumCategory_ReturnsSeedData(string url, string contentType)
        {
            _output.WriteLine("Data -> " + url);

            // Arrange
            var client = _webApplicationFactory.CreateClient();
            var seedData = _webApplicationFactory.Model.GetPopulatedModelWithSeedDataFromConfig<ForumCategory>();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var rawData = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<ForumCategoryDto>(rawData);

            Assert.Equal(seedData.Single(fc => fc.Id.Equals(responseContent.Id)).Id, responseContent.Id);
            Assert.Equal(seedData.Single(fc => fc.Id.Equals(responseContent.Id)).Name, responseContent.Name);
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetCollectionForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetCollection_ForumCategory_ReturnsCorrectContentType(string url, string contentType)
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
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetCollectionForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetCollection_ForumCategory_ReturnsSeedData(string url, int collectionSize)
        {
            _output.WriteLine("Data -> " + url);

            // Arrange
            var client = _webApplicationFactory.CreateClient();
            var seedData = _webApplicationFactory.Model.GetPopulatedModelWithSeedDataFromConfig<ForumCategory>();

            // Act
            var response = await client.GetAsync(url);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var rawData = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawData);

            List<ForumCategory> categories = new();

            foreach (var fcDb in responseContent)
            {
                categories.Add(seedData.Single(fc => fc.Id.Equals(fcDb.Id) && fc.Name.Equals(fcDb.Name)));
            }

            Assert.Equal(collectionSize, categories.Count);
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.PostSingleForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task PostSingle_ForumCategory_ReturnsCaseData(string url, string expectedCategoryName)
        {
            _output.WriteLine("Data -> " + url);

            // Arrange
            var client = _webApplicationFactory.CreateClient();
            var jsonContent = JsonConvert.SerializeObject(new { Name = expectedCategoryName });

            // Act
            var response = await client.PostAsync(url, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var rawData = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<ForumCategoryDto>(rawData);

            Assert.Equal(expectedCategoryName, responseContent.Name);
        }
    }
}
