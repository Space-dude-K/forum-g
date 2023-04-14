using Interfaces.Forum;
using Interfaces.Printer;
using Interfaces.User;

namespace Interfaces
{
    public interface IRepositoryManager
    {
        IRoleRepository UserRole { get; }
        IForumCategoryRepository ForumCategory { get; }
        IForumBaseRepository ForumBase { get; }
        IForumTopicRepository ForumTopic { get; }
        IForumPostRepository ForumPost { get; }
        IPrinterDeviceRepository PrinterDevice { get; }
        Task SaveAsync();
    }
}