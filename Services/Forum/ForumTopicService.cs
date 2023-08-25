using Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Interfaces.Forum;
using Services.Utils;
using System.Text;
using Microsoft.AspNetCore.JsonPatch;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.ForumDto;
using Entities.DTO.ForumDto.ForumView;
using System.Diagnostics;
using Interfaces.Forum.ApiServices;

namespace Services.Forum
{
    public class ForumTopicService : IForumTopicService
    {
        private readonly IHttpForumService _forumClient;
        private readonly ILoggerManager _logger;
        public ForumTopicService(ILoggerManager logger, IHttpForumService forumClient)
        {
            _logger = logger;
            _forumClient = forumClient;
        }
        public async Task<List<ForumViewTopicDto>> GetForumTopics(int categoryId, int forumId)
        {
            List<ForumViewTopicDto> forumViewTopicDtos = new List<ForumViewTopicDto>();


            var response = await _forumClient.Client.GetAsync("api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString() + "/topics");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumViewTopicDtos = JsonConvert.DeserializeObject<IEnumerable<ForumViewTopicDto>>(rawData).ToList();
            }
            else
            {
                _logger.LogError($"Unable to get topics for forum id: {forumId}");
            }

            return forumViewTopicDtos;
        }
        public async Task<List<ForumViewPostDto>> GetTopicPosts(int categoryId, int forumId, int topicId,
            int pageNumber, int pageSize, bool getAll = false)
        {
            List<ForumViewPostDto> forumViewPostDtos = new();

            bool allPosts = pageNumber == 0 && pageSize == 0;
            string uri = string.Empty;

            if (getAll)
            {
                uri = "api/categories/" + categoryId.ToString() +
                "/forums/" + forumId.ToString() +
                "/topics/" + topicId.ToString() + "/posts";
            }
            else
            {
                uri = "api/categories/" + categoryId.ToString() +
                "/forums/" + forumId.ToString() +
                "/topics/" + topicId.ToString() +
                "/posts?pageNumber=" + pageNumber.ToString() +
                "&pageSize=" + pageSize.ToString();
            }

            var response = await _forumClient.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(rawData);
                forumViewPostDtos = JsonConvert.DeserializeObject<IEnumerable<ForumViewPostDto>>(rawData).ToList();
            }
            else
            {
                _logger.LogError($"Unable to get posts for topic id: {topicId}");
            }

            return forumViewPostDtos;
        }
        public async Task<bool> UpdateTopicCounter(int categoryId, bool incresase)
        {
            bool result = false;

            string uri = "api/categories/" + categoryId.ToString();
            var response = await _forumClient.Client.GetAsync(uri + "?&fields=TotalTopics");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                int totalTopics = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawData).First().TotalTopics;

                if (incresase)
                    totalTopics++;
                else
                    totalTopics--;

                var jsonPatchObject = new JsonPatchDocument<ForumViewCategoryDto>();
                jsonPatchObject.Replace(fc => fc.TotalTopics, totalTopics);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate = await _forumClient.Client.PatchAsync(uri, new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    _logger.LogError($"Unable to update topic counter for category id: {categoryId}");
                }
            }

            return result;
        }
        public async Task<int> GetTopicCount(int categoryId)
        {
            int totalTopics = 0;

            string uri = "api/categories/" + categoryId.ToString();
            var response = await _forumClient.Client.GetAsync(uri + "?&fields=TotalTopics");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                totalTopics = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawData).First().TotalTopics;
                //totalPosts = int.Parse(JsonConvert.DeserializeObject<string>(rawData));
            }
            else
            {
                _logger.LogError($"Unable to get topic counter for category id: {categoryId}");
            }

            return totalTopics;
        }
        public async Task<bool> IncreaseViewCounterForTopic(int categoryId, int forumId, int topicId)
        {
            bool result = false;

            string uri = "api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString() + "/topics/" + topicId.ToString();
            var response = await _forumClient.Client.GetAsync(uri + "?&fields=TotalViews");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                int totalViews = JsonConvert.DeserializeObject<IEnumerable<ForumViewTopicDto>>(rawData).First().TotalViews;

                totalViews++;

                var jsonPatchObject = new JsonPatchDocument<ForumViewTopicDto>();
                jsonPatchObject.Replace(fc => fc.TotalViews, totalViews);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate =
                    await _forumClient.Client.PatchAsync(uri, new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    _logger.LogError($"Unable to view counter for topic id: {topicId}");
                }
            }

            return result;
        }
        public async Task<int> CreateForumTopic(int categoryId, int forumId, ForumTopicForCreationDto topic)
        {
            bool result = false;
            int createdTopicId = 0;
            string uri = "api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString() + "/topics";

            var jsonContent = JsonConvert.SerializeObject(topic);

            var response = await _forumClient.Client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                result = true;
                var rawData = await response.Content.ReadAsStringAsync();
                createdTopicId = JsonConvert.DeserializeObject<ForumTopicDto>(rawData).Id;
            }
            else
            {
                _logger.LogError($"Unable create topic for forum id: {forumId}");
            }

            return createdTopicId;
        }
        public async Task<bool> DeleteForumTopic(int categoryId, int forumId, int topicId)
        {
            bool result = false;
            string uri = "api/categories/" +
                categoryId.ToString() + "/forums/" +
                forumId.ToString() + "/topics/" +
                topicId.ToString();

            var response = await _forumClient.Client.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                _logger.LogError($"Unable delete topic id: {topicId}");
            }

            return result;
        }
        public async Task<bool> CreateTopicPostCounter(int topicId, ForumCounterForCreationDto forumCounterForCreationDto)
        {
            bool result = false;
            string uri = "api" +
                "/tcounters/" + topicId.ToString();

            var jsonContent = JsonConvert.SerializeObject(forumCounterForCreationDto);

            var response = await _forumClient.Client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                _logger.LogError($"Unable create topic counter for topic id: {topicId}");
            }

            return result;
        }
    }
    
}