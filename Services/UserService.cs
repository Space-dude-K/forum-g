using AutoMapper;
using Entities.DTO.UserDto;
using Entities.ViewModels;
using Forum.ViewModels;
using Interfaces;
using Interfaces.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.AspNetCore.Mvc;
using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Http.Headers;
using System.Text;
using System.Threading.Tasks;

namespace Services
{
    public class UserService : IUserService
    {
        private readonly HttpClient _client;
        public const string basePath = "api/roles";

        private readonly ILoggerManager _logger;
        private readonly IMapper _mapper;

        public UserService(HttpClient client, ILoggerManager logger, IMapper mapper)
        {
            _logger = logger;
            _mapper = mapper;
            _client = client ?? throw new ArgumentNullException(nameof(client));
        }
        public async Task<List<string>> GetUserRoles()
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var response = await _client.GetAsync(basePath);
            var rawData = await response.Content.ReadAsStringAsync();
            var responseContent = JsonConvert.DeserializeObject<IEnumerable<IdentityRole>>(rawData)
                .Select(r => r.Name).ToList();

            return responseContent;
        }
        /*public async Task<RegisterTableViewModel> GetUsersData()
        {

            var model = new RegisterTableViewModel()
            { 
                RegisterViewModels = 
                };

            return result.IsSuccessStatusCode;
        }*/
    }
}
