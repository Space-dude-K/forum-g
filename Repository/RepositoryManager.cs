using Contracts;
using Entities;

namespace Repository
{
    public class RepositoryManager : IRepositoryManager
    {
        private ForumContext _forumContext;
        private IForumUserRepository _forumUserRepository;

        public RepositoryManager(ForumContext forumContext)
        {
            _forumContext = forumContext;
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
        public void Save() => _forumContext.SaveChanges();
    }
}