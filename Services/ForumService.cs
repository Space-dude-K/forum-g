using Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Interfaces.Forum;
using Services.Utils;
using Entities.DTO.ForumDto.ForumView;
using Entities.ViewModels.Forum;
using System.Diagnostics;
using System.Text;
using Microsoft.AspNetCore.JsonPatch;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.UserDto;
using Entities.DTO.ForumDto;

namespace Services
{
    public class ForumService : IForumService
    {
        private readonly HttpClient _client;
        private readonly ILoggerManager _logger;

        public ForumService(HttpClient client, ILoggerManager logger,
            IAuthenticationService authenticationService)
        {
            _logger = logger;
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var tokenResponse = authenticationService.Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" }).Result;
            var parsedTokenStr = tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr.Result);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);
        }
        public async Task<ForumHomeViewModel> GetForumCategoriesAndForumBasesForModel()
        {
            ForumHomeViewModel forumHomeViewModel = new();
            forumHomeViewModel.Categories = await GetForumCategories();
            
            for(int i = 0; i < forumHomeViewModel.Categories.Count; i++)
            {
                forumHomeViewModel.Categories[i].Forums = await GetForumBases(forumHomeViewModel.Categories[i].Id);

                int postCount = await GetTopicPostCount(forumHomeViewModel.Categories[i].Id);
                forumHomeViewModel.Categories[i].TotalPosts = postCount;

                int topicCount = await GetTopicCount(forumHomeViewModel.Categories[i].Id);
                forumHomeViewModel.Categories[i].TotalTopics = topicCount;

                /*
                for (int j = 0; j < forumHomeViewModel.Categories[i].Forums.Count; j++)
                {
                    var topics = await GetForumTopics(forumHomeViewModel.Categories[i].Id, forumHomeViewModel.Categories[i].Forums[j].Id);
                    forumHomeViewModel.Categories[i].Forums[j].TopicsCount = topics.Count;

                    
                    forumHomeViewModel.Categories[i].Forums[j].TotalPosts = postCount;



                    for (int k = 0; k < topics.Count; k++)
                    {
                        var posts = await GetTopicPosts(
                            forumHomeViewModel.Categories[i].Id, 
                            forumHomeViewModel.Categories[i].Forums[j].Id, 
                            topics[k].Id, 0, 0);
                        forumHomeViewModel.Categories[i].Forums[j].TotalPosts += posts.Count;
                    }
                }*/
            }

            return forumHomeViewModel;
        }
        public async Task<ForumBaseViewModel> GetForumTopicsForModel(int categoryId, int forumId)
        {
            ForumBaseViewModel forumHomeViewModel = new();
            //forumHomeViewModel.ForumTitle = forumTitle;
            forumHomeViewModel.Topics = await GetForumTopics(categoryId, forumId);

            return forumHomeViewModel;
        }
        public async Task<ForumTopicViewModel> GetTopicPostsForModel(int categoryId, int forumId, int topicId, int pageNumber, int pageSize)
        {
            ForumTopicViewModel forumHomeViewModel = new();
            var topicAuthor = await GetForumUser(topicId);
            forumHomeViewModel.SubTopicAuthor = topicAuthor.FirstAndLastNames;

            var topics = await GetForumTopics(categoryId, forumId);
            forumHomeViewModel.TopicId = topicId;
            forumHomeViewModel.SubTopicCreatedAt = topics.FirstOrDefault(t => t.Id == topicId).CreatedAt.Value.ToShortDateString();
            forumHomeViewModel.TotalPosts = await GetTopicPostCount(topicId);

            // Default paging to latest topic message.
            if(pageNumber == 0 && forumHomeViewModel.TotalPages > 1)
            {
                pageNumber = forumHomeViewModel.TotalPages;
            }

            forumHomeViewModel.Posts = await GetTopicPosts(categoryId, forumId, topicId, pageNumber, pageSize);
            
            var postUserTask = forumHomeViewModel.Posts.Select(async p => p.ForumUser = await GetForumUser(p.ForumUserId));

            await Task.WhenAll(postUserTask);

            return forumHomeViewModel;
        }

        public async Task<List<ForumViewCategoryDto>> GetForumCategories()
        {
            List<ForumViewCategoryDto> forumViewCategoryDtos = new List<ForumViewCategoryDto>();


            var response = await _client.GetAsync("api/categories");
            
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
        public async Task<List<ForumViewBaseDto>> GetForumBases(int categoryId)
        {
            List<ForumViewBaseDto> forumViewBaseDtos = new();

            var response = await _client.GetAsync("api/categories/" + categoryId.ToString() + "/forums");
            
            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumViewBaseDtos = JsonConvert.DeserializeObject<IEnumerable<ForumViewBaseDto>>(rawData).ToList();
            }
            else
            {
                _logger.LogError($"Unable to get forums for category id: {categoryId}");
            }

            return forumViewBaseDtos;
        }
        public async Task<List<ForumViewTopicDto>> GetForumTopics(int categoryId, int forumId)
        {
            List<ForumViewTopicDto> forumViewTopicDtos = new List<ForumViewTopicDto>();


            var response = await _client.GetAsync("api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString() + "/topics");

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
        public async Task<List<ForumViewPostDto>> GetTopicPosts(int categoryId, int forumId, int topicId, int pageNumber, int pageSize)
        {
            List<ForumViewPostDto> forumViewPostDtos = new();

            bool allPosts = pageNumber == 0 && pageSize == 0;
            string uri = string.Empty;

            if (false)
            {
                uri = "api/categories/" + categoryId.ToString() +
                "/forums/" + forumId.ToString() +
                "/topics/" + topicId.ToString();
            }
            else
            {
                uri = "api/categories/" + categoryId.ToString() +
                "/forums/" + forumId.ToString() +
                "/topics/" + topicId.ToString() +
                "/posts?pageNumber=" + pageNumber.ToString() +
                "&pageSize=" + pageSize.ToString();
            }

            var response = await _client.GetAsync(uri);

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
        public async Task<int> GetTopicPostCount(int topicId)
        {
            int totalPosts = 0;

            string uri = "api" +
                "/tcounters/" + topicId.ToString();
            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                totalPosts = JsonConvert.DeserializeObject<IEnumerable<ForumTopicCounterDto>>(rawData).First().PostCounter;
            }
            else
            {
                _logger.LogError($"Unable to get topic post counter for topic id: {topicId}");
            }

            return totalPosts;
        }
        public async Task<int> GetTopicCount(int categoryId)
        {
            int totalTopics = 0;

            string uri = "api/categories/" + categoryId.ToString();
            var response = await _client.GetAsync(uri + "?&fields=TotalTopics");

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
        public async Task<ForumUserDto> GetForumUser(int userId)
        {
            ForumUserDto forumUser = new();

            string uri = "api/users/" + userId.ToString();
            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumUser = JsonConvert.DeserializeObject<IEnumerable<ForumUserDto>>(rawData).First();
            }
            else
            {
                _logger.LogError($"Unable to get forum user id: {userId}");
            }

            return forumUser;
        }
        public async Task<int> GetPostCounterForUser(int userId)
        {
            int totalPosts = 0;

            string uri = "api/usersf/" + userId.ToString();
            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                totalPosts = JsonConvert.DeserializeObject<ForumUserDto>(rawData).TotalPostCounter;
            }
            else
            {
                _logger.LogError($"Unable to get post counter for user id: {userId}");
            }

            return totalPosts;
        }

        // COUNTERS
        public async Task<bool> UpdatePostCounter(int topicId, bool incresase)
        {
            bool result = false;

            string uri = "api" +
                "/tcounters/" + topicId.ToString();
            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                int totalPosts = JsonConvert.DeserializeObject<IEnumerable<ForumTopicCounterDto>>(rawData).First().PostCounter;

                if (incresase)
                    totalPosts++;
                else
                    totalPosts--;

                JsonPatchDocument<ForumTopicCounterDto> jsonPatchObject = new();
                jsonPatchObject.Replace(fc => fc.PostCounter, totalPosts);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate = await _client.PatchAsync(uri, new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

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
        public async Task<bool> UpdateTopicCounter(int categoryId, bool incresase)
        {
            bool result = false;

            string uri = "api/categories/" + categoryId.ToString();
            var response = await _client.GetAsync(uri + "?&fields=TotalTopics");

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
                var responseAfterUpdate = await _client.PatchAsync(uri, new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

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
        public async Task<bool> UpdatePostCounterForUser(int userId, bool incresase)
        {
            bool result = false;

            string uri = "api/usersf/" + userId.ToString();
            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                int totalPosts = JsonConvert.DeserializeObject<ForumUserDto>(rawData).TotalPostCounter;

                if (incresase)
                    totalPosts++;
                else
                    totalPosts--;

                JsonPatchDocument<ForumUserDto> jsonPatchObject = new();
                jsonPatchObject.Replace(fc => fc.TotalPostCounter, totalPosts);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate = await _client.PatchAsync(uri, new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

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
        public async Task<bool> IncreaseViewCounterForForumBase(int categoryId, int forumId)
        {
            bool result = false;

            string uri = "api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString();
            var response = await _client.GetAsync(uri + "?&fields=TotalViews");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                int totalViews = JsonConvert.DeserializeObject<IEnumerable<ForumViewBaseDto>>(rawData).First().TotalViews;

                totalViews++;

                var jsonPatchObject = new JsonPatchDocument<ForumViewBaseDto>();
                jsonPatchObject.Replace(fc => fc.TotalViews, totalViews);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate = await _client.PatchAsync(uri, new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

                if(responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    _logger.LogError($"Unable to view counter for category id: {categoryId}");
                }
            }

            return result;
        }
        public async Task<bool> IncreaseViewCounterForTopic(int categoryId, int forumId, int topicId)
        {
            bool result = false;

            string uri = "api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString() + "/topics/" + topicId.ToString();
            var response = await _client.GetAsync(uri + "?&fields=TopicViewCounter");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                int totalViews = JsonConvert.DeserializeObject<IEnumerable<ForumViewTopicDto>>(rawData).First().TotalViews;

                totalViews++;

                var jsonPatchObject = new JsonPatchDocument<ForumViewTopicDto>();
                jsonPatchObject.Replace(fc => fc.TotalViews, totalViews);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate = 
                    await _client.PatchAsync(uri, new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

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

        // POST
        public async Task<bool> CreateForumBase(int categoryId, ForumBaseForCreationDto forum)
        {
            bool result = false;

            string uri = "api/categories/" + categoryId.ToString() + "/forums";

            var jsonContent = JsonConvert.SerializeObject(forum);

            var response = await _client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                _logger.LogError($"Unable create forum for category id: {categoryId}");
            }

            return result;
        }
        public async Task<bool> CreateForumCategory(ForumCategoryForCreationDto category)
        {
            bool result = false;
            string uri = "api/categories/";

            var jsonContent = JsonConvert.SerializeObject(category);

            var response = await _client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

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
        public async Task<bool> CreateForumTopic(int categoryId, int forumId, ForumTopicForCreationDto topic)
        {
            bool result = false;
            string uri = "api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString() + "/topics";

            var jsonContent = JsonConvert.SerializeObject(topic);

            var response = await _client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                _logger.LogError($"Unable create topic for forum id: {forumId}");
            }

            return result;
        }
        public async Task<bool> CreateForumPost(int categoryId, int forumId, int topicId, ForumPostForCreationDto post)
        {
            bool result = false;
            string uri = "api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString() + "/topics/" + topicId.ToString() + "/posts";

            var jsonContent = JsonConvert.SerializeObject(post);

            var response = await _client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

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

        // DELETE
        public async Task<bool> DeleteForumPost(int categoryId, int forumId, int topicId, int postId)
        {
            bool result = false;
            string uri = "api/categories/" + 
                categoryId.ToString() + "/forums/" + 
                forumId.ToString() + "/topics/" + 
                topicId.ToString() + "/posts/" + 
                postId.ToString();

            var response = await _client.DeleteAsync(uri);

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

        // UPDATE
        public async Task<bool> UpdatePost(int categoryId, int forumId, int topicId, int postId, string newText)
        {
            bool result = false;

            string uri = "api/categories/" +
                categoryId.ToString() + "/forums/" +
                forumId.ToString() + "/topics/" +
                topicId.ToString() + "/posts/" +
                postId.ToString();
            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                var postDto = JsonConvert.DeserializeObject<IEnumerable<ForumPostDto>>(rawData).First();

                postDto.PostText = newText;
                postDto.UpdatedAt = DateTime.Now;
                postDto.ForumUserId = 1;

                var jsonAfterUpdade = JsonConvert.SerializeObject(postDto);
                var responseAfterUpdate =
                    await _client.PutAsync(uri, new StringContent(jsonAfterUpdade, Encoding.UTF8, "application/json"));

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
    }
}