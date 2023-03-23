using Entities.Models.Forum;

namespace Web_api_tests.Tests.Forum
{
    public static class TestCases
    {
        public static IEnumerable<object[]> GetTestDataForForumCategory()
        {
            return new List<object[]>  
            {
                new object[] 
                { 
                    new ForumCategory()
                    {
                        Id = 1,
                        Name = "Test category 1",
                        ForumUserId = 1
                    } 
                },
                new object[] 
                { 
                    new ForumCategory()
                    {
                        Id = 2,
                        Name = "Test category 2",
                        ForumUserId = 2
                    } 
                } 
            };
        }
        /*public static IEnumerable<object[]> GetTestDataForForumBase()
        {
            return new List<object[]>
            {
                new object[]
                {
                    new ForumBase()
                    {
                        Id = 1,
                        Name = "Test category 1",
                        ForumUserId = 1
                    }
                },
                new object[]
                {
                    new ForumBase()
                    {
                        Id = 2,
                        Name = "Test category 2",
                        ForumUserId = 2
                    }
                }
            };
        }*/
    }
}