using Interfaces;
using Newtonsoft.Json;
using Interfaces.Forum;
using System.Text;
using Entities.DTO.UserDto;
using Entities.DTO.FileDto;

namespace Services.Forum
{
    // TODO. Refactoring
    public class ForumService : IForumService
    {
        private readonly IHttpForumService _forumClient;
        private readonly ILoggerManager _logger;
        public ForumService(ILoggerManager logger, IHttpForumService forumClient)
        {
            _logger = logger;
            _forumClient = forumClient;
        }

        public async Task<ForumFileDto> GetForumFileByUserId(int forumUserId)
        {
            ForumFileDto forumFileDtoFromDb = new();

            string uri = "api/file/" + forumUserId.ToString();
            var response = await _forumClient.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumFileDtoFromDb = JsonConvert.DeserializeObject<ForumFileDto>(rawData);
            }
            else
            {
                _logger.LogInfo($"Missing forum file for user id: {forumUserId}");
            }

            return forumFileDtoFromDb;
        }
        public async Task<ForumUserDto> GetForumUser(int userId)
        {
            ForumUserDto forumUser = new();

            string uri = "api/users/" + userId.ToString();
            var response = await _forumClient.Client.GetAsync(uri);

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
        
        public async Task<bool> CreateForumFile(ForumFileDto file)
        {
            bool result = false;
            string uri = "api/file";

            var jsonContent = JsonConvert.SerializeObject(file);

            var response = await _forumClient.Client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                _logger.LogError($"Unable create file for user id: {file.ForumUserId}");
            }

            return result;
        }
        public async Task<bool> UpdateForumFile(int forumUserId, ForumFileDto forumFileDto)
        {
            bool result = false;

            string uri = "api/file/" + forumUserId.ToString();
            var response = await _forumClient.Client.GetAsync(uri);

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                var forumFileDtoFromDb = JsonConvert.DeserializeObject<ForumFileDto>(rawData);
                forumFileDtoFromDb.Path = forumFileDto.Path;
                forumFileDtoFromDb.Name = forumFileDto.Name;

                var jsonAfterUpdade = JsonConvert.SerializeObject(forumFileDtoFromDb);
                var responseAfterUpdate =
                    await _forumClient.Client.PutAsync(uri, new StringContent(jsonAfterUpdade, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
            }
            else
            {
                _logger.LogError($"Unable update file with user id: {forumUserId}");
            }

            return result;
        }
    }
}