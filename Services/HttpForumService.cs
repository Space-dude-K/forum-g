using Interfaces;
using Interfaces.Forum;
using Newtonsoft.Json;
using Services.Utils;
using System.Net.Http.Headers;

namespace Services
{
    public class HttpForumService : IHttpForumService
    {
        public HttpClient Client { get; }
        private readonly ILoggerManager _logger;

        public HttpForumService(HttpClient httpClient, ILoggerManager logger, IAuthenticationService authenticationService)
        {
            Client = httpClient;
            _logger = logger;
            Client = httpClient ?? throw new ArgumentNullException(nameof(httpClient));
            Client.DefaultRequestHeaders.Accept.Add(new MediaTypeWithQualityHeaderValue("application/json"));

            var tokenResponse = authenticationService.Login(new Entities.ViewModels.LoginViewModel()
            { UserName = "Admin", Password = "1234567890" }).Result;

            var parsedTokenStr = tokenResponse.Content.ReadAsStringAsync();
            var parsedToken = JsonConvert.DeserializeObject<BearerToken>(parsedTokenStr.Result);
            Client.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", parsedToken.Token);
        }
    }
}