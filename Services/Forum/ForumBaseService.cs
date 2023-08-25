using Interfaces;
using Newtonsoft.Json;
using System.Net.Http.Headers;
using Interfaces.Forum;
using Services.Utils;
using Entities.DTO.ForumDto.ForumView;
using System.Text;
using Microsoft.AspNetCore.JsonPatch;
using Entities.DTO.ForumDto.Create;
using Interfaces.Forum.ApiServices;

namespace Services.Forum
{
    public class ForumBaseService : IForumBaseService
    {
        private readonly IHttpForumService _forumClient;
        private readonly ILoggerManager _logger;
        public ForumBaseService(ILoggerManager logger, IHttpForumService forumClient)
        {
            _logger = logger;
            _forumClient = forumClient;
        }
        public async Task<bool> CreateForumBase(int categoryId, ForumBaseForCreationDto forum)
        {
            bool result = false;
            
            string uri = "api/categories/" + categoryId.ToString() + "/forums";

            var jsonContent = JsonConvert.SerializeObject(forum);

            var response = await _forumClient.Client.PostAsync(uri, new StringContent(jsonContent, Encoding.UTF8, "application/json"));

            if (response.IsSuccessStatusCode)
            {
                result = true;
            }
            else
            {
                _logger.LogError($"Unable create forum for category id: {categoryId}");
            }

            return result;
        }
        public async Task<bool> IncreaseViewCounterForForumBase(int categoryId, int forumId)
        {
            bool result = false;

            string uri = "api/categories/" + categoryId.ToString() + "/forums/" + forumId.ToString();
            var response = await _forumClient.Client.GetAsync(uri + "?&fields=TotalViews");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                int totalViews = JsonConvert.DeserializeObject<IEnumerable<ForumViewBaseDto>>(rawData).First().TotalViews;

                totalViews++;

                var jsonPatchObject = new JsonPatchDocument<ForumViewBaseDto>();
                jsonPatchObject.Replace(fc => fc.TotalViews, totalViews);

                var jsonAfterUpdated = JsonConvert.SerializeObject(jsonPatchObject);
                var responseAfterUpdate = await _forumClient.Client.PatchAsync(uri, new StringContent(jsonAfterUpdated, Encoding.UTF8, "application/json"));

                if (responseAfterUpdate.IsSuccessStatusCode)
                {
                    result = true;
                }
                else
                {
                    _logger.LogError($"Unable to view counter for category id: {categoryId}");
                }
            }

            return result;
        }
        public async Task<List<ForumViewBaseDto>> GetForumBases(int categoryId)
        {
            List<ForumViewBaseDto> forumViewBaseDtos = new();

            var response = await _forumClient.Client.GetAsync("api/categories/" + categoryId.ToString() + "/forums");

            if (response.IsSuccessStatusCode)
            {
                var rawData = await response.Content.ReadAsStringAsync();
                forumViewBaseDtos = JsonConvert.DeserializeObject<IEnumerable<ForumViewBaseDto>>(rawData).ToList();
            }
            else
            {
                _logger.LogError($"Unable to get forums for category id: {categoryId}");
            }

            return forumViewBaseDtos;
        }
    }
}
