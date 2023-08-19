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

namespace Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;

        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public UserService(HttpClient client, ILoggerManager logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
        }
        public async Task<List<string>> GetUserRoles()
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

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
        
    }
}
