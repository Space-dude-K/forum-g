using Forum.ViewModels;

namespace Interfaces.Forum
{
    public interface IAuthenticationService
    {
        Task<HttpResponseMessage> Register(RegisterViewModel model);
    }
}