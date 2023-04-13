using AutoMapper;
using Contracts;
using Contracts.Forum;
using Entities.DTO.UserDto;
using Forum.ViewModels;
using Newtonsoft.Json;
using System.Text;

namespace Forum.Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;
        public const string basePath = "api/authentication";

        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public AuthenticationService(HttpClient client, ILoggerManager logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }
        public async Task<bool> Register(RegisterViewModel model)
        {
            var userDto = _mapper.Map<UserForRegistrationDto>(model);
            var userJson = JsonConvert.SerializeObject(userDto);
            var postContent = new StringContent(userJson, Encoding.UTF8, "application/json");
            var result = await _client.PostAsync("api/authentication", postContent);

            return result.IsSuccessStatusCode;
        }
    }
}