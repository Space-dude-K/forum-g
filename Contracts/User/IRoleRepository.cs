using Microsoft.AspNetCore.Identity;

namespace Interfaces.User
{
    public interface IRoleRepository
    {
        Task<List<IdentityRole>> GetAllRolesAsync(bool trackChanges);
    }
}