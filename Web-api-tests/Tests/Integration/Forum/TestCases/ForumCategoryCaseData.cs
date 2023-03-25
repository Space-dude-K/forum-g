using Entities.DTO.ForumDto.Create;

namespace ForumTest.Tests.Integration.Forum.TestCases
{
    public class ForumCategoryCaseData
    {
        public static IEnumerable<object[]> GetAllCategoriesData =>
            new List<object[]>
            {
                new object[] { "/api/categories", "application/json; charset=utf-8" }
            };
        public static IEnumerable<object[]> GetSingleForumCategoryData =>
            new List<object[]>
            {
                new object[] { "/api/categories/1", "application/json; charset=utf-8" },
                new object[] { "/api/categories/2", "application/json; charset=utf-8" }
            };
        public static IEnumerable<object[]> GetCollectionForumCategoryData =>
            new List<object[]>
            {
                new object[] { "/api/categories/collection/(1,2)", 2 },
                new object[] { "/api/categories/collection/(2,3)", 2 },
                new object[] { "/api/categories/collection/(3,4)", 2 }
            };
        public static IEnumerable<object[]> PostSingleForumCategoryData =>
            new List<object[]>
            {
                new object[] { "/api/categories", "Test category name 1" },
                new object[] { "/api/categories", "Test category name 2" },
            };
    }
}