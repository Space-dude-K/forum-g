using Entities.Models;
using Entities.RequestFeatures.User;

namespace Interfaces.User
{
    public interface IUserDataRepository
    {
        Task<List<AppUser>> GetAllUsersAsync(UserParameters userParameters, bool trackChanges);
        Task<List<AppUser>> GetUserAsync(string userId, UserParameters userParameters, bool trackChanges);
    }
}