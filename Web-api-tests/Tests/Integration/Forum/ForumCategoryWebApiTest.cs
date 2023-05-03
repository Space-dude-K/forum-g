using Azure.Core;
using Entities;
using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto.Update;
using Entities.Models.Forum;
using ForumTest.Extensions;
using ForumTest.Tests.Integration.Forum.TestCases;

using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.JsonPatch;
using Newtonsoft.Json;
using System.Dynamic;
using System.Net;
using System.Net.Http.Headers;
using System.Security.Claims;
using System.Text;
using Xunit.Abstractions;

namespace ForumTest.Tests.Integration.Forum
{
    public class ForumCategoryWebApiTest
    {
        private readonly ITestOutputHelper _output;

        public ForumCategoryWebApiTest(ITestOutputHelper output)
        {
            _output = output;
        }

        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetAllCategoriesData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetAll_ForumCategories_ReturnsCorrectContentType(string uri, string contentType)
        {
            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var response = await client.GetAsync(uri);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(contentType, response.Content.Headers.ContentType.ToString());
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetAllCategoriesData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetAll_ForumCategories_ReturnsSeedData(string uri, string contentType)
        {
            _output.WriteLine("uri -> " + uri + " Type: " + contentType);

            // Arrange
            var fac = new TestWithEfInMemoryDb<ForumContext>();
            var client = fac.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var seedData = fac.Model.GetPopulatedModelWithSeedDataFromConfig<ForumCategory>();

            // Act
            var response = await client.GetAsync(uri);

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
        public async Task GetSingle_ForumCategory_ReturnsCorrectContentType(string uri, string contentType)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var response = await client.GetAsync(uri);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(contentType, response.Content.Headers.ContentType.ToString());
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetSingleForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetSingle_ForumCategory_ReturnsSeedData(string uri, string contentType)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var fac = new TestWithEfInMemoryDb<ForumContext>();
            var client = fac.CreateClient();
            dynamic data = new ExpandoObject();
            data.sub = Guid.NewGuid();
            data.role = new[] { "sub_role", "Administrator" };
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            //client.SetFakeBearerToken((object)data);
            //client.SetFakeJwtBearerToken("TestUser", new[] { "Administrator" });
            //client.SetFakeJwtBearerToken(claims1);
            //_output.WriteLine("uri -> " + client.);


            var seedData = fac.Model.GetPopulatedModelWithSeedDataFromConfig<ForumCategory>();

            // Act
            var response = await client.GetAsync(uri);
            
            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var rawData = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawData);

            Assert.Equal(seedData.Single(fc => fc.Id.Equals(responseContent.First().Id)).Id, responseContent.First().Id);
            Assert.Equal(seedData.Single(fc => fc.Id.Equals(responseContent.First().Id)).Name, responseContent.First().Name);
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetCollectionForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetCollection_ForumCategory_ReturnsCorrectContentType(string uri, string contentType, int collectionSize)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var response = await client.GetAsync(uri);

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.Equal(HttpStatusCode.OK, response.StatusCode);
            Assert.Equal(contentType, response.Content.Headers.ContentType.ToString());
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.GetCollectionForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task GetCollection_ForumCategory_ReturnsSeedData(string uri, string contentType, int collectionSize)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var fac = new TestWithEfInMemoryDb<ForumContext>();
            var client = fac.CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var seedData = fac.Model.GetPopulatedModelWithSeedDataFromConfig<ForumCategory>();

            // Act
            var response = await client.GetAsync(uri);

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
        [MemberData(nameof(ForumCategoryCaseData.UpdateSingleForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task PutSingle_ForumCategory_ReturnsUpdatedCaseData(string uri, string expectedCategoryName)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var claims = new Dictionary<string, object>
            {
                { ClaimTypes.Name, "test@sample.com" },
                { ClaimTypes.Role, "Administrator" },
                { "http://mycompany.com/customClaim", "someValue" },
            };
            var client = new TestWithEfInMemoryDb<ForumContext>().CreateClient();
            //client.SetFakeJwtBearerToken(claims);
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var responseGetBeforeUp = await client.GetAsync(uri);
            var rawDataBeforeUp = await responseGetBeforeUp.Content.ReadAsStringAsync();
            var responseContentBeforeUp = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawDataBeforeUp);
            responseContentBeforeUp.First().Name = responseContentBeforeUp.First().Name + " updated";
            var jsonContentBeforeUp = JsonConvert.SerializeObject(responseContentBeforeUp.First());
            var response = await client.PutAsync(uri, new StringContent(jsonContentBeforeUp, Encoding.UTF8, "application/json"));
            var responseGetAfterUp = await client.GetAsync(uri);
            var rawDataAfterUp = await responseGetAfterUp.Content.ReadAsStringAsync();
            var responseContentAfterUp = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawDataAfterUp);

            // Assert
            responseGetBeforeUp.EnsureSuccessStatusCode(); // Status Code 200-299
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            responseGetAfterUp.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.Equal(expectedCategoryName, responseContentAfterUp.First().Name);
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.UpdateSingleForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task PatchSingle_ForumCategory_ReturnsUpdatedCaseData(string uri, string expectedCategoryName)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var responseGetBeforeUp = await client.GetAsync(uri);
            var rawDataBeforeUp = await responseGetBeforeUp.Content.ReadAsStringAsync();
            var responseContentBeforeUp = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawDataBeforeUp);
            responseContentBeforeUp.First().Name += " updated";

            var jsonPatchObject = new JsonPatchDocument<ForumCategoryForUpdateDto>();
            jsonPatchObject.Replace(fc => fc.Name, responseContentBeforeUp.First().Name);

            var jsonContentBeforeUp = JsonConvert.SerializeObject(jsonPatchObject);
            var response = await client.PatchAsync(uri, new StringContent(jsonContentBeforeUp, Encoding.UTF8, "application/json"));
            var responseGetAfterUp = await client.GetAsync(uri);
            var rawDataAfterUp = await responseGetAfterUp.Content.ReadAsStringAsync();
            var responseContentAfterUp = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawDataAfterUp);

            // Assert
            responseGetBeforeUp.EnsureSuccessStatusCode(); // Status Code 200-299
            response.EnsureSuccessStatusCode(); // Status Code 200-299
            responseGetAfterUp.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.Equal(expectedCategoryName, responseContentAfterUp.First().Name);
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.PostSingleForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task PostSingle_ForumCategory_ReturnsCaseData(string uri, string expectedCategoryName)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            var jsonContent = JsonConvert.SerializeObject(new { Name = expectedCategoryName });
            _output.WriteLine("jsonContent -> " + jsonContent);
            // Act
            var response = await client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var rawData = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<ForumCategoryDto>(rawData);

            Assert.Equal(expectedCategoryName, responseContent.Name);
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.PostSingleForumCategoryDataErrors), MemberType = typeof(ForumCategoryCaseData))]
        public async Task PostSingle_ForumCategory_ReturnsUprocessableEntity(string uri, string expectedError)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().CreateClient();
            var jsonContent = JsonConvert.SerializeObject(new { });

            // Act
            var response = await client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            string body = string.Empty;
            using (var reader = new StreamReader(response.Content.ReadAsStream()))
            {
                body = await reader.ReadToEndAsync();
            }

            _output.WriteLine("Body -> " + body);

            // Assert
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal(expectedError, body);
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.PostCollectionForumCategoryDataErrors), MemberType = typeof(ForumCategoryCaseData))]
        public async Task PostCollection_ForumCategory_ReturnsUprocessableEntities(string uri, 
            List<ForumCategoryForCreationDto> expectedCategoryNames, string expectedError)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().CreateClient();
            var jsonContent = JsonConvert.SerializeObject(expectedCategoryNames);
            expectedError = "{" + string.Join(",", expectedCategoryNames.Select((e, i) => expectedError
            .Insert(1, "[" + i.ToString() + "]."))) + "}";

            _output.WriteLine("Json -> " + jsonContent);

            // Act
            var response = await client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            string body = string.Empty;
            using (var reader = new StreamReader(response.Content.ReadAsStream()))
            {
                body = await reader.ReadToEndAsync();
            }

            _output.WriteLine("body -> " + body);

            // Assert
            Assert.Equal(HttpStatusCode.UnprocessableEntity, response.StatusCode);
            Assert.Equal(expectedError, body);
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.PostCollectionForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task PostCollection_ForumCategory_ReturnsCaseData(string uri, List<ForumCategoryForCreationDto> expectedCategoryNames)
        {
            _output.WriteLine("uri -> " + uri);

            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().CreateClient();
            var jsonContent = JsonConvert.SerializeObject(expectedCategoryNames);

            _output.WriteLine("Json -> " + jsonContent);

            // Act
            var response = await client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            // Assert
            response.EnsureSuccessStatusCode(); // Status Code 200-299

            var rawData = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawData);

            for(int i = 0; i < responseContent.Count(); i++)
            {
                Assert.Equal(expectedCategoryNames[i].Name, responseContent.ToArray()[i].Name);
            }

            Assert.Equal(expectedCategoryNames.Count(), responseContent.Count());
        }
        [Theory]
        [MemberData(nameof(ForumCategoryCaseData.DeleteSingleForumCategoryData), MemberType = typeof(ForumCategoryCaseData))]
        public async Task DeleteSingle_ForumCategory_ReturnsNotFound(string uri)
        {
            // Arrange
            var client = new TestWithEfInMemoryDb<ForumContext>().CreateClient();
            client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            // Act
            var responseGetBeforeDel = await client.GetAsync(uri);
            var responseDelete = await client.DeleteAsync(uri);
            var responseGetAfterDel = await client.GetAsync(uri);

            // Assert
            responseGetBeforeDel.EnsureSuccessStatusCode(); // Status Code 200-299
            responseDelete.EnsureSuccessStatusCode(); // Status Code 200-299

            Assert.Equal(HttpStatusCode.OK, responseGetBeforeDel.StatusCode);
            Assert.Equal(HttpStatusCode.NotFound, responseGetAfterDel.StatusCode);
        }
    }
}