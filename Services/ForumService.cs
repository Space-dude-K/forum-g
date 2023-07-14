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

                for (int j = 0; j < forumHomeViewModel.Categories[i].Forums.Count; j++)
                {
                    var topics = await GetForumTopics(forumHomeViewModel.Categories[i].Id, forumHomeViewModel.Categories[i].Forums[j].Id);
                    forumHomeViewModel.Categories[i].Forums[j].TopicsCount = topics.Count;

                    for (int k = 0; k < topics.Count; k++)
                    {
                        var posts = await GetTopicPosts(forumHomeViewModel.Categories[i].Id, forumHomeViewModel.Categories[i].Forums[j].Id, topics[k].Id);
                        forumHomeViewModel.Categories[i].Forums[j].TotalPosts += posts.Count;
                    }
                }
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
        public async Task<ForumTopicViewModel> GetTopicPostsForModel(int categoryId, int forumId, int topicId)
        {
            ForumTopicViewModel forumHomeViewModel = new();
            forumHomeViewModel.Posts = await GetTopicPosts(categoryId, forumId, topicId);
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
        public async Task<List<ForumViewPostDto>> GetTopicPosts(int categoryId, int forumId, int topicId)
        {
            List<ForumViewPostDto> forumViewPostDtos = new();

            var tokenResponse =
                await authenticationService.Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);

            string uri = "api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString() + "/topics/" + topicId.ToString() + "/posts";
            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                Debug.WriteLine(rawData);
                forumViewPostDtos = JsonConvert.DeserializeObject<IEnumerable<ForumViewPostDto>>(rawData).ToList();
            }

            return forumViewPostDtos;
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
    }
}