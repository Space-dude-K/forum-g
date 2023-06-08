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
        }
        public async Task<List<ForumViewCategoryDto>> GetForumCategories()
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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
    }
}
