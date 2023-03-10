using Blazored.LocalStorage;
using InterviewApp.Helper;
using InterviewApp.Models;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;


namespace InterviewApp.Services
{
    public class AuthService : IAuthService
    {
        private readonly HttpClient _httpClient;
        private readonly AuthenticationStateProvider _authenticationStateProvider;
        private readonly ILocalStorageService _localStorage;
        public AuthService(HttpClient httpClient, AuthenticationStateProvider authenticationStateProvider,
            ILocalStorageService localStorage)
        {
            _httpClient = httpClient;
            _authenticationStateProvider = authenticationStateProvider;
            _localStorage = localStorage;
        }

        public async Task<LoginResult> Login(LoginViewModel loginModel)
        {
            var techniciens = await _httpClient.GetFromJsonAsync<Technicien[]>("sample-data/techniciens.json");
            var tech = techniciens.FirstOrDefault(t => t.UserName == loginModel.UserName && t.Password == loginModel.Password);
            if (tech == null)
            {
                return new LoginResult { Successful = false, Error = "Nom d'utilisateur ou mot de passe incorrect" };
            }
            var res = new LoginResult
            {
                Successful = true,
                Token = "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiIxIiwibmFtZSI6InRlY2huaWNpZW5fMSIsImp0aSI6Ijg1YmY0NjNjLTA3NzEtNGU1NC04MzNkLWNkYmFjZjcyZjAxZiIsImh0dHA6Ly9zY2hlbWFzLnhtbHNvYXAub3JnL3dzLzIwMDUvMDUvaWRlbnRpdHkvY2xhaW1zL25hbWVpZGVudGlmaWVyIjoiMSIsImV4cCI6MTY3ODYzNzM0MSwiaXNzIjoiaHR0cHM6Ly9sb2NhbGhvc3Q6NDQzODgiLCJhdWQiOiJodHRwczovL2xvY2FsaG9zdDo0NDM4OCJ9.SrxYTrKYB8ZD-neNKw7vBo0IkrrYpLXT4VGE-6m4hXA"
            };
            await _localStorage.SetItemAsync("authToken", res?.Token);
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsAuthenticated(loginModel.UserName);
            
            return res;
        }

        public async Task Logout()
        {
            await _localStorage.RemoveItemAsync("authToken");
            ((ApiAuthenticationStateProvider)_authenticationStateProvider).MarkUserAsLoggedOut();
            _httpClient.DefaultRequestHeaders.Authorization = null;
        }

       
    }
}
