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
    }

}