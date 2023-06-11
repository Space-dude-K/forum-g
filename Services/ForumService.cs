using AutoMapper;
using Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Entities.DTO.ForumDto;
using Interfaces.Forum;
using Microsoft.AspNetCore.Authentication;
using System.Net.Http;
using Entities.DTO.UserDto;
using Services.Utils;
using Entities.DTO.ForumDto.ForumView;
using Entities.ViewModels.Forum;

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
        public async Task<ForumHomeViewModel> GetForumCategoriesAndForumBases()
        {
            ForumHomeViewModel forumHomeViewModel = new ForumHomeViewModel();
            forumHomeViewModel.Categories = await GetForumCategories();
            
            for(int i = 0; i < forumHomeViewModel.Categories.Count; i++)
            {
                forumHomeViewModel.Categories[i].Forums = await GetForumBases(forumHomeViewModel.Categories[i].Id);
            }

            return forumHomeViewModel;
        }
        public async Task<List<ForumViewCategoryDto>> GetForumCategories()
        {
            var tokenResponse = 
                await authenticationService.Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);

            var response = await _client.GetAsync("api/categories");
            var rawData = await response.Content.ReadAsStringAsync();

            var responseContent = JsonConvert.DeserializeObject<IEnumerable<ForumViewCategoryDto>>(rawData).ToList();

            return responseContent;
        }
        public async Task<List<ForumViewBaseDto>> GetForumBases(int categoryId)
        {
            var tokenResponse =
                await authenticationService.Login(new Entities.ViewModels.LoginViewModel() { UserName = "Admin", Password = "1234567890" });
            var parsedTokenStr = await tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr);

            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);

            var response = await _client.GetAsync("api/categories/" + categoryId.ToString() + "/forums");
            var rawData = await response.Content.ReadAsStringAsync();

            var responseContent = JsonConvert.DeserializeObject<IEnumerable<ForumViewBaseDto>>(rawData).ToList();

            return responseContent;
        }
    }
}
