using Interfaces;
using Interfaces.Forum;
using Interfaces.Printer;
using Entities;
using Repository.Forum;
using Repository.Printer;
using Interfaces.User;
using Repository.User;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private ForumContext _forumContext;
        private PrinterContext _printerContext;

        private IRoleRepository _roleRepository;

        private IForumCategoryRepository _forumCategoryRepository;
        private IForumBaseRepository _forumBaseRepository;
        private IForumTopicRepository _forumTopicRepository;
        private IForumPostRepository _forumPostRepository;

        private IPrinterDeviceRepository _printerDeviceRepository;

        public RepositoryManager(ForumContext forumContext, PrinterContext printerContext)
        {
            _forumContext = forumContext;
            _printerContext = printerContext;
        }
        public IRoleRepository UserRole
        {
            get
            {
                if (_roleRepository == null)
                    _roleRepository = new RoleRepository(_forumContext);
                return _roleRepository;
            }
        }

        public IForumCategoryRepository ForumCategory
        {
            get
            {
                if (_forumCategoryRepository == null)
                    _forumCategoryRepository = new ForumCategoryRepository(_forumContext);
                return _forumCategoryRepository;
            }
        }

        public IForumBaseRepository ForumBase
        {
            get
            {
                if (_forumBaseRepository == null)
                    _forumBaseRepository = new ForumBaseRepository(_forumContext);
                return _forumBaseRepository;
            }
        }

        public IForumTopicRepository ForumTopic
        {
            get
            {
                if (_forumTopicRepository == null)
                    _forumTopicRepository = new ForumTopicRepository(_forumContext);
                return _forumTopicRepository;
            }
        }

        public IForumPostRepository ForumPost
        {
            get
            {
                if (_forumPostRepository == null)
                    _forumPostRepository = new ForumPostRepository(_forumContext);
                return _forumPostRepository;
            }
        }
        public IPrinterDeviceRepository PrinterDevice
        {
            get
            {
                if (_printerDeviceRepository == null)
                    _printerDeviceRepository = new PrinterDeviceRepository(_printerContext);
                return _printerDeviceRepository;
            }
        }

        public Task SaveAsync()
        {
            if(_forumContext.ChangeTracker.HasChanges())
                return _forumContext.SaveChangesAsync();

            if (_printerContext.ChangeTracker.HasChanges())
                return _printerContext.SaveChangesAsync();

            return Task.CompletedTask;
        }
    }
}