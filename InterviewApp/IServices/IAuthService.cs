

using InterviewApp.Models;

namespace InterviewApp.Services
{
    public interface IAuthService
    {
        Task<LoginResult> Login(LoginViewModel loginModel);
        Task Logout();

    }
}
