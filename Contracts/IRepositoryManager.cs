namespace Contracts
{
    public interface IRepositoryManager
    {
        IForumUserRepository ForumUser { get; }
        void Save();
    }
}