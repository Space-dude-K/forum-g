using Entities;
using Interfaces.User;
using Microsoft.AspNetCore.Identity;
using Microsoft.EntityFrameworkCore;

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