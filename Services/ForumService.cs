using AutoMapper;
using Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Interfaces.Forum;
using Services.Utils;
using Entities.DTO.ForumDto.ForumView;
using Entities.ViewModels.Forum;
using System.Diagnostics;
using System.Linq;
using System.Text;
using System;
using Entities.DTO.ForumDto.Update;
using Microsoft.AspNetCore.JsonPatch;
using Entities;
using System.Security.Policy;
using Entities.DTO.ForumDto.Create;
using Entities.DTO.UserDto;
using Entities.Models.Forum;
using Entities.DTO.ForumDto;

namespace Services
{
    public class ForumService : IForumService
    {
        private readonly HttpClient _client;

        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly Interfaces.Forum.IAuthenticationService authenticationService;

        public ForumService(HttpClient client, ILoggerManager logger, IMapper mapper, 
            Interfaces.Forum.IAuthenticationService authenticationService)
        {
            _logger = logger;
            _mapper = mapper;
            this.authenticationService = authenticationService;
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
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
            forumHomeViewModel.TotalPosts = await GetTopicPostCount(categoryId);

            // Default paging to latest topic message.
            if(pageNumber != 1 && forumHomeViewModel.TotalPages > 1)
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

            var tokenResponse = 
                await authenticationService.Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);

            var response = await _client.GetAsync("api/categories");
            
            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumViewCategoryDtos = JsonConvert.DeserializeObject<IEnumerable<ForumViewCategoryDto>>(rawData).ToList();
            }

            return forumViewCategoryDtos;
        }
        public async Task<List<ForumViewBaseDto>> GetForumBases(int categoryId)
        {
            List<ForumViewBaseDto> forumViewBaseDtos = new();

            var tokenResponse =
                await authenticationService.Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);

            var response = await _client.GetAsync("api/categories/" + categoryId.ToString() + "/forums");
            
            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumViewBaseDtos = JsonConvert.DeserializeObject<IEnumerable<ForumViewBaseDto>>(rawData).ToList();
            }

            return forumViewBaseDtos;
        }
        public async Task<List<ForumViewTopicDto>> GetForumTopics(int categoryId, int forumId)
        {
            List<ForumViewTopicDto> forumViewTopicDtos = new List<ForumViewTopicDto>();

            var tokenResponse =
                await authenticationService.Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);

            var response = await _client.GetAsync("api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString() + "/topics");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumViewTopicDtos = JsonConvert.DeserializeObject<IEnumerable<ForumViewTopicDto>>(rawData).ToList();
            }

            return forumViewTopicDtos;
        }
        public async Task<List<ForumViewPostDto>> GetTopicPosts(int categoryId, int forumId, int topicId, int pageNumber, int pageSize)
        {
            List<ForumViewPostDto> forumViewPostDtos = new();

            var tokenResponse =
                await authenticationService.Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);


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

            return forumViewPostDtos;
        }
        public async Task<int> GetTopicPostCountViaApiCall(int categoryId, int forumId, int topicId)
        {
            int totalPosts = 0;

            var tokenResponse =
                await authenticationService.Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);

            string uri = uri = "api/categories/" + categoryId.ToString() +
                "/forums/" + forumId.ToString() +
                "/topics/" + topicId.ToString() + "/posts/count";

            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                totalPosts = JsonConvert.DeserializeObject<int>(rawData);
            }

            return totalPosts;
        }
        public async Task<int> GetTopicPostCount(int categoryId)
        {
            int totalPosts = 0;

            var tokenResponse =
                await authenticationService.Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);

            string uri = "api/categories/" + categoryId.ToString();
            var response = await _client.GetAsync(uri + "?&fields=TotalPosts");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                totalPosts = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawData).First().TotalPosts;
                //totalPosts = int.Parse(JsonConvert.DeserializeObject<string>(rawData));
            }

            return totalPosts;
        }
        public async Task<int> GetTopicCount(int categoryId)
        {
            int totalTopics = 0;

            var tokenResponse =
                await authenticationService.Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);

            string uri = "api/categories/" + categoryId.ToString();
            var response = await _client.GetAsync(uri + "?&fields=TotalTopics");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                totalTopics = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawData).First().TotalTopics;
                //totalPosts = int.Parse(JsonConvert.DeserializeObject<string>(rawData));
            }

            return totalTopics;
        }
        public async Task<ForumUserDto> GetForumUser(int userId)
        {
            ForumUserDto forumUser = new();

            var tokenResponse =
                await authenticationService.Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);

            string uri = "api/users/" + userId.ToString();
            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumUser = JsonConvert.DeserializeObject<IEnumerable<ForumUserDto>>(rawData).First();
            }

            return forumUser;
        }

        // COUNTERS
        public async Task<bool> UpdatePostCounter(int categoryId, bool incresase)
        {
            bool result = false;

            var tokenResponse =
                await authenticationService.
                Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);


            string uri = "api/categories/" + categoryId.ToString();
            var response = await _client.GetAsync(uri + "?&fields=TotalPosts");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                int totalPosts = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawData).First().TotalPosts;

                if (incresase)
                    totalPosts++;
                else
                    totalPosts--;

                var jsonPatchObject = new JsonPatchDocument<ForumViewCategoryDto>();
                jsonPatchObject.Replace(fc => fc.TotalPosts, totalPosts);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate = await _client.PatchAsync(uri, new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
            }

            return result;
        }
        public async Task<bool> IncreaseTopicCounter(int categoryId)
        {
            bool result = false;

            var tokenResponse =
                await authenticationService.
                Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);


            string uri = "api/categories/" + categoryId.ToString();
            var response = await _client.GetAsync(uri + "?&fields=TotalTopics");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                int totalTopics = JsonConvert.DeserializeObject<IEnumerable<ForumCategoryDto>>(rawData).First().TotalTopics;

                totalTopics++;

                var jsonPatchObject = new JsonPatchDocument<ForumViewCategoryDto>();
                jsonPatchObject.Replace(fc => fc.TotalTopics, totalTopics);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate = await _client.PatchAsync(uri, new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
            }

            return result;
        }
        public async Task<bool> IncreaseViewCounterForForumBase(int categoryId, int forumId)
        {
            bool result = false;

            var tokenResponse =
                await authenticationService.
                Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);


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
            }

            return result;
        }
        public async Task<bool> IncreaseViewCounterForTopic(int categoryId, int forumId, int topicId)
        {
            bool result = false;

            var tokenResponse =
                await authenticationService.
                Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);

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
            }

            return result;
        }

        // POST
        public async Task<bool> CreateForumBase(int categoryId, ForumBaseForCreationDto forum)
        {
            var tokenResponse =
                await authenticationService.Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);
            string uri = "api/categories/" + categoryId.ToString() + "/forums";

            var jsonContent = JsonConvert.SerializeObject(forum);

            var response = await _client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> CreateForumCategory(ForumCategoryForCreationDto category)
        {
            var tokenResponse =
                await authenticationService.Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);
            string uri = "api/categories/";

            var jsonContent = JsonConvert.SerializeObject(category);

            var response = await _client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> CreateForumTopic(int categoryId, int forumId, ForumTopicForCreationDto topic)
        {
            var tokenResponse =
                await authenticationService.Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);
            string uri = "api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString() + "/topics";

            var jsonContent = JsonConvert.SerializeObject(topic);

            var response = await _client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }
        public async Task<bool> CreateForumPost(int categoryId, int forumId, int topicId, ForumPostForCreationDto post)
        {
            var tokenResponse =
                await authenticationService.Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);
            string uri = "api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString() + "/topics/" + topicId.ToString() + "/posts";

            var jsonContent = JsonConvert.SerializeObject(post);

            var response = await _client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            return response.IsSuccessStatusCode;
        }

        // DELETE
        public async Task<bool> DeleteForumPost(int categoryId, int forumId, int topicId, int postId)
        {
            var tokenResponse =
                await authenticationService.Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);
            string uri = "api/categories/" + 
                categoryId.ToString() + "/forums/" + 
                forumId.ToString() + "/topics/" + 
                topicId.ToString() + "/posts/" + 
                postId.ToString();

            var response = await _client.DeleteAsync(uri);

            return response.IsSuccessStatusCode;
        }
    }
}