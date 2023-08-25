using Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Interfaces.Forum;
using Services.Utils;
using Entities.DTO.ForumDto.ForumView;
using System.Text;
using Entities.DTO.ForumDto.Create;
using Interfaces.Forum.ApiServices;

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
        public async Task<List<ForumViewCategoryDto>> GetForumCategories()
        {
            List<ForumViewCategoryDto> forumViewCategoryDtos = new List<ForumViewCategoryDto>();


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
    }

}
