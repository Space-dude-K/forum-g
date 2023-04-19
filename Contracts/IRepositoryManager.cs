using Interfaces.Forum;
using Interfaces.Printer;
using Interfaces.User;

namespace Interfaces
{
    public interface IRepositoryManager
    {
        IUserDataRepository Users { get; }
        IForumCategoryRepository ForumCategory { get; }
        IForumBaseRepository ForumBase { get; }
        IForumTopicRepository ForumTopic { get; }
        IForumPostRepository ForumPost { get; }
        IPrinterDeviceRepository PrinterDevice { get; }
        Task SaveAsync();
    }
}