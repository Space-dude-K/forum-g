﻿using Interfaces.Forum;
using Interfaces.Printer;
using Interfaces.User;

namespace Interfaces
{
    public interface IRepositoryManager
    {
        IRoleRepository Roles { get; }
        IUserDataRepository Users { get; }
        IForumCategoryRepository ForumCategory { get; }
        IForumBaseRepository ForumBase { get; }
        IForumTopicRepository ForumTopic { get; }
        IForumTopicCounterRepository ForumTopicCounter { get; }
        IForumPostRepository ForumPost { get; }
        IPrinterDeviceRepository PrinterDevice { get; }
        IForumUserDataRepository ForumUsers { get; }

        Task SaveAsync();
    }
}