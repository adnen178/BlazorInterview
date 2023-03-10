using AntDesign;
using Microsoft.AspNetCore.Components;
using Blazored.LocalStorage;
using InterviewApp.Services;
using InterviewApp.Models;

namespace InterviewApp.Pages
{
    public class LoginBase : ComponentBase
    {
        [Inject]
        public IAuthService AuthService { get; set; }
        [Inject]
        public NavigationManager NavigationManager { get; set; }
        [Inject]
        public ILocalStorageService localStorage { get; set; }
        protected LoginViewModel loginModel { get; set; } = new LoginViewModel();
        protected bool ShowErrors { get; set; }
        protected string Error { get; set; } = "";
        protected bool loading { get; set; } =false ;

        protected async Task HandleLogin()
        {
            loading = true;
            ShowErrors = false;

            var result = await AuthService.Login(loginModel);

            if (result.Successful)
            {
                NavigationManager.NavigateTo("/",true);
            }
            else
            {
                Error = result.Error;
                ShowErrors = true;
                //await _message.Error(result.Error.ToString(),5);
                loading = false;
            }
        }
    }
}
