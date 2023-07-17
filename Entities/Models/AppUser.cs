using Entities.Models.Forum;
using Microsoft.AspNetCore.Identity;

namespace Entities.Models
{
    public class AppUser : IdentityUser<int>
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Cabinet { get; set; }
        public string InternalPhone { get; set; }
        public string BirthDate { get; set; }
        public DateTime? CreatedAt { get; set; }
        public virtual ForumUser ForumUser { get; set; }
    }
}