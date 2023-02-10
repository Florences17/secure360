using Blazored.Toast;
using Microsoft.AspNetCore.Components.Web;
using Microsoft.AspNetCore.Components.WebAssembly.Hosting;
using Opteamix.AuthorizationFramework.BlazorApp;
using Opteamix.AuthorizationFramework.BlazorApp.Services;

var builder = WebAssemblyHostBuilder.CreateDefault(args);
builder.RootComponents.Add<App>("#app");
builder.RootComponents.Add<HeadOutlet>("head::after");

builder.Services.AddScoped(sp => new HttpClient { BaseAddress = new Uri("https://localhost:7201/") });
builder.Services.AddScoped<CommonService>();
builder.Services.AddScoped<IModuleService, ModuleService>();
builder.Services.AddScoped<IApplicationService, ApplicationService>();

builder.Services.AddScoped<IPrivilegeService, PrivilegeService>();
builder.Services.AddScoped<IClientService, ClientService>();
builder.Services.AddScoped<IAddRoleService, AddRoleService>();
builder.Services.AddScoped<IUserService, UserService>();

builder.Services.AddBlazoredToast();

//builder.Services.AddScoped(x => {
//    var apiUrl = new Uri(builder.Configuration["API_URL"]);

//    return new HttpClient() { BaseAddress = apiUrl };
//});

await builder.Build().RunAsync();
