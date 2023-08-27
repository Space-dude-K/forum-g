using Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Interfaces.Forum;
using Services.Utils;
using System.Text;
using Microsoft.AspNetCore.JsonPatch;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.UserDto;
using Entities.DTO.ForumDto;
using Interfaces.Forum.ApiServices;

namespace Services.Forum
{
    public class ForumPostService : IForumPostService
    {
        private readonly IHttpForumService _forumClient;
        private readonly ILoggerManager _logger;
        public ForumPostService(ILoggerManager logger, IHttpForumService forumClient)
        {
            _logger = logger;
            _forumClient = forumClient;
        }

        public async Task<int> GetTopicPostCount(int topicId)
        {
            int totalPosts = 0;

            string uri = "api" +
                "/tcounters/" + topicId.ToString();
            var response = await _forumClient.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                totalPosts = JsonConvert.DeserializeObject<ForumTopicCounterDto>(rawData).PostCounter;
            }
            else
            {
                _logger.LogError($"Unable to get topic post counter for topic id: {topicId}");
            }

            return totalPosts;
        }
        public async Task<bool> UpdatePost(int categoryId, int forumId, int topicId, int postId, string newText)
        {
            bool result = false;

            string uri = "api/categories/" +
                categoryId.ToString() + "/forums/" +
                forumId.ToString() + "/topics/" +
                topicId.ToString() + "/posts/" +
                postId.ToString();
            var response = await _forumClient.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                var postDto = JsonConvert.DeserializeObject<IEnumerable<ForumPostDto>>(rawData).First();

                postDto.PostText = newText;
                postDto.UpdatedAt = DateTime.Now;
                postDto.ForumUserId = 1;

                var jsonAfterUpdade = JsonConvert.SerializeObject(postDto);
                var responseAfterUpdate =
                    await _forumClient.Client.PutAsync(uri, new StringContent(jsonAfterUpdade, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
            }
            else
            {
                _logger.LogError($"Unable update post with id: {postId}");
            }

            return result;
        }
        public async Task<bool> DeleteForumPost(int categoryId, int forumId, int topicId, int postId)
        {
            bool result = false;
            string uri = "api/categories/" +
                categoryId.ToString() + "/forums/" +
                forumId.ToString() + "/topics/" +
                topicId.ToString() + "/posts/" +
                postId.ToString();

            var response = await _forumClient.Client.DeleteAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                _logger.LogError($"Unable delete post for topic id: {topicId}");
            }

            return result;
        }
        public async Task<bool> CreateForumPost(int categoryId, int forumId, int topicId, ForumPostForCreationDto post)
        {
            bool result = false;
            string uri = "api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString() + "/topics/" + topicId.ToString() + "/posts";

            var jsonContent = JsonConvert.SerializeObject(post);

            var response = await _forumClient.Client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                _logger.LogError($"Unable create post for topic id: {topicId}");
            }

            return result;
        }
        public async Task<bool> UpdatePostCounter(int topicId, bool incresase, int postCountToDelete = 0)
        {
            bool result = false;

            string uri = "api" +
                "/tcounters/" + topicId.ToString();
            var response = await _forumClient.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();

                // Nothing to update. Post deleted.
                if (string.IsNullOrEmpty(rawData))
                    return true;

                int totalPosts = JsonConvert.DeserializeObject<ForumTopicCounterDto>(rawData).PostCounter;

                if (incresase)
                    totalPosts++;
                else
                {
                    if (postCountToDelete > 0)
                        totalPosts = -postCountToDelete;
                    else
                        totalPosts--;
                }

                JsonPatchDocument<ForumTopicCounterDto> jsonPatchObject = new();
                jsonPatchObject.Replace(fc => fc.PostCounter, totalPosts);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate = await _forumClient.Client.PatchAsync(uri, new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    _logger.LogError($"Unable to update post counter for topic id: {topicId}");
                }
            }

            return result;
        }
        public async Task<bool> UpdatePostCounterForUser(int userId, bool incresase, int postCountToDelete = 0)
        {
            bool result = false;

            string uri = "api/usersf/" + userId.ToString();
            var response = await _forumClient.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                int totalPosts = JsonConvert.DeserializeObject<ForumUserDto>(rawData).TotalPostCounter;

                if (incresase)
                    totalPosts++;
                else
                {
                    if (postCountToDelete > 0)
                        totalPosts -= postCountToDelete;
                    else
                        totalPosts--;
                }

                JsonPatchDocument<ForumUserDto> jsonPatchObject = new();
                jsonPatchObject.Replace(fc => fc.TotalPostCounter, totalPosts);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate = await _forumClient.Client.PatchAsync(uri, new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    _logger.LogError($"Unable to update post counter for user id: {userId}");
                }
            }

            return result;
        }
    }
}