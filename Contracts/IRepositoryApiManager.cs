using Interfaces.Forum.API;
using Repository.API.Forum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces
{
    public interface IRepositoryApiManager
    {
        IForumBaseApiRepository ForumApis { get; }
        IForumCategoryApiRepository CategoryApis { get; }
        IForumTopicApiRepository TopicApis { get; }
        IForumPostApiRepository PostApis { get; }
        IForumFileApiRepository FileApis { get; }
        IForumUserApiRepository ForumUserApis { get; }
    }
}
