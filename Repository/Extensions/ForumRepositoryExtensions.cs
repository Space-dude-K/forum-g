using Entities.Models.Forum;

namespace Repository.Extensions
{
    public static class ForumRepositoryExtensions
    {
        public static IQueryable<ForumPost> FilterPosts(this IQueryable<ForumPost> posts, uint minLikes, uint maxLikes)
        {
            return posts.Where(e => (e.Likes >= minLikes && e.Likes <= maxLikes));
        }

        public static IQueryable<ForumPost> Search(this IQueryable<ForumPost> posts, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return posts;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return posts.Where(e => e.PostName.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<ForumTopic> Search(this IQueryable<ForumTopic> topics, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return topics;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return topics.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<ForumBase> Search(this IQueryable<ForumBase> forums, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return forums;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return forums.Where(e => e.ForumTitle.ToLower().Contains(lowerCaseTerm));
        }
        public static IQueryable<ForumCategory> Search(this IQueryable<ForumCategory> categories, string searchTerm)
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return categories;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return categories.Where(e => e.Name.ToLower().Contains(lowerCaseTerm));
        }
        // TODO
        public static IQueryable<T> SearchGeneric<T>(this IQueryable<T> data, string searchField, string searchTerm) where T : class
        {
            if (string.IsNullOrWhiteSpace(searchTerm))
                return data;

            var lowerCaseTerm = searchTerm.Trim().ToLower();

            return data.Where(e => e.GetType().GetProperty(searchField).GetValue(e).ToString().ToLower().Contains(lowerCaseTerm)).AsQueryable<T>();
        }
    }
}