using AutoMapper;
using Entities.DTO.UserDto;
using Entities.ViewModels;
using Interfaces;
using Interfaces.User;
using Microsoft.AspNetCore.Authentication;
using Microsoft.AspNetCore.Identity;
using Newtonsoft.Json;
using Services.Utils;
using System.Net.Http.Headers;
using Interfaces.Forum;
using System.Text;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;

        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly Interfaces.Forum.IAuthenticationService _authenticationService;

        public UserService(HttpClient client, ILoggerManager logger, IMapper mapper, Interfaces.Forum.IAuthenticationService authenticationService)
        {
            _logger = logger;
            _mapper = mapper;
            _authenticationService = authenticationService;
            _client = client ?? throw new ArgumentNullException(nameof(client));
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var tokenResponse = _authenticationService.Login(new LoginViewModel()
            { UserName = "Admin", Password = "1234567890" }).Result;

            var parsedTokenStr = tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr.Result);
            _client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);
        }
        public async Task<List<string>> GetUserRoles()
        {
            var response = await _client.GetAsync("api/roles");
            var rawData = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<IEnumerable<IdentityRole>>(rawData)
                .Select(r => r.Name).ToList();

            return responseContent;
        }
        // Only for testing
        public async Task<RegisterTableViewModel> GetUsersData()
        {
            var response = await _client.GetAsync("api/users");
            var rawData = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<IEnumerable<ForumUserDto>>(rawData).ToList();

            var model = new RegisterTableViewModel()
            {
                AppUsers = responseContent
            };

            return model;
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
        public async Task<ForumUserDto> GetForumUserDto(int userId)
        {
            ForumUserDto forumUserDto = new();

            string uri = "api/usersf/" + userId.ToString();
            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumUserDto = JsonConvert.DeserializeObject<ForumUserDto>(rawData);
            }
            else
            {
                _logger.LogError($"Unable to get post counter for user id: {userId}");
            }

            return forumUserDto;
        }
        public async Task<bool> UpdateAppUser(int userId, AppUserDto appUserDto)
        {
            bool result = false;

            string uri = "api/usersa/" + userId.ToString();
            var response = await _client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                var appUserFromDb = JsonConvert.DeserializeObject<AppUserDto>(rawData);
                appUserFromDb = appUserDto;

                var jsonAfterUpdade = JsonConvert.SerializeObject(appUserFromDb);
                var responseAfterUpdate =
                    await _client.PutAsync(uri, new StringContent(jsonAfterUpdade, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
            }
            else
            {
                _logger.LogError($"Unable update app user id: {userId}");
            }

            return result;
        }
    }
}
