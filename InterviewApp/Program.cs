using Blazored.LocalStorage;
using Blazored.SessionStorage;
using InterviewApp.Helper;
using InterviewApp.IServices;
using InterviewApp.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Authorization;

var builder = WebApplication.CreateBuilder(args);

// Add services to the container.
builder.Services.AddRazorPages();
builder.Services.AddServerSideBlazor();
builder.Services.AddAuthorizationCore();

builder.Services.AddAntDesign();
builder.Services.AddHttpClient<IAuthService, AuthService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("URL")["BaseUri"]);
});
builder.Services.AddHttpClient<IGearService, GearService>(client =>
{
    client.BaseAddress = new Uri(builder.Configuration.GetSection("URL")["BaseUri"]);
});
builder.Services.AddScoped<AuthenticationStateProvider, ApiAuthenticationStateProvider>();
builder.Services.AddBlazoredLocalStorage();
builder.Services.AddBlazoredSessionStorage();
var app = builder.Build();

// Configure the HTTP request pipeline.
if (!app.Environment.IsDevelopment())
{
    app.UseExceptionHandler("/Error");
    // The default HSTS value is 30 days. You may want to change this for production scenarios, see https://aka.ms/aspnetcore-hsts.
    app.UseHsts();
}

app.UseHttpsRedirection();

app.UseStaticFiles();

app.UseRouting();

app.UseAuthentication();
app.UseAuthorization();

app.MapBlazorHub();
app.MapFallbackToPage("/_Host");

app.Run();
