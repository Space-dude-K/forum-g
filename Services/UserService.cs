using AutoMapper;
using Entities.DTO.UserDto;
using Forum.ViewModels;
using Interfaces;
using Interfaces.User;
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
        public async Task<HttpResponseMessage> GetUserRoles()
        {
            _client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));
            return await _client.GetAsync(basePath);
        }
    }
}
