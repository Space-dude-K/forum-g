using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Entities.DTO.UserDto
{
    public class AppUserDto
    {
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public int? Cabinet { get; set; }
        public int? InternalPhone { get; set; }
        public string? BirthDate { get; set; }
        public string? Division { get; set; }
        public string? Company { get; set; }
    }
}
