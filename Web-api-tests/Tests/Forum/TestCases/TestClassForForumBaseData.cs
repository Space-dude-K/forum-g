using Entities.Models.Forum;
using Microsoft.EntityFrameworkCore.Infrastructure;
using Microsoft.EntityFrameworkCore.Metadata;
using System.Collections;
using Web_api_tests.Extensions;

namespace Web_api_tests.Tests.Forum.TestCases
{
    public class TestClassForForumBaseData : TestWithSqlite, IEnumerable<object[]>
    {
        private IEnumerable<object[]> data;

        public TestClassForForumBaseData()
        {
            data = DbContext.GetService<IDesignTimeModel>().Model.GetPopulatedModelWithSeedDataFromConfigForTestCase<ForumBase>();
        }
        public IEnumerator<object[]> GetEnumerator()
        { return data.GetEnumerator(); }

        IEnumerator IEnumerable.GetEnumerator()
        { return GetEnumerator(); }
    }
}