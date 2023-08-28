using Interfaces;
using Interfaces.Forum;
using Microsoft.Extensions.Logging;

namespace Repository
{
    public abstract class RepositoryApi<T, TLogger, THttpClient> : 
        IRepositoryApi<T> 
        where T : class
        where TLogger : ILoggerManager
        where THttpClient : IHttpForumService
    {
        protected ILoggerManager _logger;
        protected IHttpForumService _httpForumService;
        public RepositoryApi(ILoggerManager logger, IHttpForumService httpForumService)
        {
            _logger = logger;
            _httpForumService = httpForumService;
        }
    }
}