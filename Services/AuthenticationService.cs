using AutoMapper;
using Interfaces;
using Interfaces.Forum;
using Entities.DTO.UserDto;
using Forum.ViewModels;
using Newtonsoft.Json;
using System.Text;
using Microsoft.AspNetCore.Identity;
using Entities.Models;
using Entities.DTO.UserDto.Create;
using Microsoft.Extensions.Configuration;
using Entities.ViewModels;

namespace Services
{
    public class AuthenticationService : IAuthenticationService
    {
        private readonly HttpClient _client;

        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;
        private readonly UserManager<AppUser> _userManager;
        private readonly IConfiguration _configuration;

        private AppUser _user;

        public AuthenticationService(HttpClient client, ILoggerManager logger,IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }
        public async Task<HttpResponseMessage> Login(LoginViewModel model)
        {
            var loginDto = _mapper.Map<UserForAuthenticationDto>(model);
            var loginJson = JsonConvert.SerializeObject(loginDto);
            var postContent = new StringContent(loginJson, Encoding.UTF8, "application/json");
            var result = await _client.PostAsync("api/authentication/login", postContent);

            return result;
        }
        public async Task<HttpResponseMessage> Register(RegisterViewModel model)
        {
            var userDto = _mapper.Map<UserForCreationDto>(model);
            var userJson = JsonConvert.SerializeObject(userDto);
            var postContent = new StringContent(userJson, Encoding.UTF8, "application/json");
            var result = await _client.PostAsync("api/authentication", postContent);

            return result;
        }
    }
}