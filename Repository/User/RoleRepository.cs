using Interfaces.User;
using Entities;
using Microsoft.EntityFrameworkCore;
using Microsoft.AspNetCore.Identity;

namespace Repository.User
{
    public class RoleRepository : RepositoryBase<IdentityRole, ForumContext>, IRoleRepository
    {
        public RoleRepository(ForumContext forumContext) : base(forumContext)
        {
        }
        public async Task<List<IdentityRole>> GetAllRolesAsync(bool trackChanges)
        {
            var roles = await FindAll(trackChanges)
                .ToListAsync();

            return roles;
        }
    }
}