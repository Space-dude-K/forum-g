using Contracts;
using Contracts.Forum;
using Contracts.Printer;
using Entities;
using Repository.Forum;
using Repository.Printer;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private ForumContext _forumContext;
        private PrinterContext _printerContext;

        private IForumUserRepository _forumUserRepository;
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
        public IForumUserRepository ForumUser
        {
            get
            {
                if (_forumUserRepository == null)
                    _forumUserRepository = new ForumUserRepository(_forumContext);
                return _forumUserRepository;
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

        public void Save()
        {
            if(_forumContext.ChangeTracker.HasChanges())
                _forumContext.SaveChanges();

            if (_printerContext.ChangeTracker.HasChanges())
                _printerContext.SaveChanges();
        }
    }
}