using Entities.Models;
using Microsoft.AspNetCore.Identity;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Interfaces.User
{
    public interface IRoleRepository
    {
        Task<List<AppRole>> GetAllRolesAsync(bool trackChanges);
    }
}
