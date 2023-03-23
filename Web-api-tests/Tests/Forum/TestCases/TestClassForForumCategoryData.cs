using Entities.Models.Forum;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Web_api_tests.Extensions;

namespace Web_api_tests.Tests.Forum.TestCases
{
    public class TestClassForForumCategoryData : TestWithSqlite, IEnumerable<object[]>
    {
        private IEnumerable<object[]> data;

        public TestClassForForumCategoryData()
        {
            data = DbContext.GetService<IDesignTimeModel>().Model.GetPopulatedModelWithSeedDataFromConfigForTestCase<ForumCategory>();
        }
        public IEnumerator<object[]> GetEnumerator()
        { return data.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }
    }
}