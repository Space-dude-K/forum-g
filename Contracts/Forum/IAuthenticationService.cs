using Forum.ViewModels;

namespace Interfaces.Forum
{
    public interface IAuthenticationService
    {
        Task<bool> Register(RegisterViewModel model);
    }
}