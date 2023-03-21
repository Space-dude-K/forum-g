namespace Contracts.Forum
{
    public interface IForumCore<T>
    {
        Task<IEnumerable<T>> GetAllAsync(bool trackChanges);
        Task<T> GetAsync(int categoryId, bool trackChanges);
        void Create(T category);
        Task<IEnumerable<T>> GetByIdsAsync(IEnumerable<int> ids, bool trackChanges);
        void Delete(T category);
    }
}