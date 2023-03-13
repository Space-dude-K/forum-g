using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.Models
{
    public class ForumAccount
    {
        public int Id { get; set; }
        public int AccountTypeId { get; set; }
        public string Login { get; set; }
        public string Password { get; set; }
        public string Ip { get; set; }

        public virtual ForumUser ForumUser { get; set; }
        public virtual ForumAccountType ForumAccountType { get; set; }
    }
}
