using Forum.ViewModels;

namespace Contracts.Forum
{
    public interface IAuthenticationService
    {
        Task<bool> Register(RegisterViewModel model);
    }
}