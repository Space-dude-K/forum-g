using Interfaces;
using Newtonsoft.Json;
using Interfaces.Forum;
using Entities.DTO.ForumDto.ForumView;
using System.Text;
using Entities.DTO.ForumDto.Create;
using Interfaces.Forum.ApiServices;
using Entities.DTO.ForumDto;
using Microsoft.AspNetCore.JsonPatch;

namespace Services.Forum
{
    public class ForumCategoryService : IForumCategoryService
    {
        private readonly IHttpForumService _forumClient;
        private readonly ILoggerManager _logger;
        public ForumCategoryService(ILoggerManager logger, IHttpForumService forumClient)
        {
            _logger = logger;
            _forumClient = forumClient;
        }
        public async Task<List<ForumViewCategoryDto>> GetForumCategories()
        {
            List<ForumViewCategoryDto> forumViewCategoryDtos = new();

            var response = await _forumClient.Client.GetAsync("api/categories");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumViewCategoryDtos = JsonConvert.DeserializeObject<IEnumerable<ForumViewCategoryDto>>(rawData).ToList();
            }
            else
            {
                _logger.LogError($"Unable to get categories");
            }

            return forumViewCategoryDtos;
        }
        public async Task<bool> CreateForumCategory(ForumCategoryForCreationDto category)
        {
            bool result = false;
            string uri = "api/categories/";

            var jsonContent = JsonConvert.SerializeObject(category);

            var response = await _forumClient.Client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                _logger.LogError($"Unable create category for category id: {category.Name}");
            }

            return result;
        }
        public async Task<bool> UpdateTotalPostCounterForCategory(int categoryId, bool incresase, int postCountToDelete = 0)
        {
            bool result = false;

            string uri = "api/categories/" + categoryId.ToString();
            var response = await _forumClient.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                int totalPosts = JsonConvert.DeserializeObject<ICollection<ForumCategoryDto>>(rawData).First().TotalPosts;

                if (incresase)
                    totalPosts++;
                else
                {
                    if (postCountToDelete > 0)
                        totalPosts = -postCountToDelete;
                    else
                        totalPosts--;
                }

                JsonPatchDocument<ForumCategoryDto> jsonPatchObject = new();
                jsonPatchObject.Replace(fc => fc.TotalPosts, totalPosts);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate = await _forumClient.Client.PatchAsync(uri, 
                    new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    _logger.LogError($"Unable to update total post counter for category id: {categoryId}");
                }
            }

            return result;
        }
    }
}