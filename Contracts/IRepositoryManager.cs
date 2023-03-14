namespace Contracts
{
    public interface IRepositoryManager
    {
        IForumUserRepository ForumUser { get; }
        IForumCategoryRepository ForumCategory { get; }
        IForumBaseRepository ForumBase { get; }
        IForumTopicRepository ForumTopic { get; }
        IForumPostRepository ForumPost { get; }
        void Save();
    }
}