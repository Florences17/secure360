using Microsoft.AspNetCore.Components;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Dropdown;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Modules;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Roles;
using Opteamix.AuthorizationFramework.BlazorApp.Services;

namespace Opteamix.AuthorizationFramework.BlazorApp.Pages.Reports
{
    public partial class Reports
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        [Inject]
        public CommonService commonService { get; set; }

        [Inject]
        IApplicationService ApplicationService { get; set; }

        [Inject]
        IModuleService ModuleService { get; set; }

        [Inject]
        IAddRoleService AddRoleService { get; set; }

        public enum LoadingContainerState { Loading, Loaded, Error }

        LoadingContainerState application_state;

        public IEnumerable<Models.Applications.Application> Applications { get; set; }

        public List<Models.Applications.Application> DisplayApplications { get; set; } = new List<Models.Applications.Application>();

        public Models.Applications.Application SelectedApplication { get; set; }

        public IEnumerable<Module> Modules { get; set; }

        public List<Dropdown> DisplayModules { get; set; } = new List<Dropdown>();

        public IEnumerable<FetchRolesModel>? Roles { get; set; }

        public List<Dropdown> DisplayRoles { get; set; } = new List<Dropdown>();

        public string ModuleSelected { get; set; } = 0.ToString();

        public string RoleSelected { get; set; } = 0.ToString();

        public async Task SetSelectedApplication(Models.Applications.Application application)
        {
            SelectedApplication = application;
            SelectedApplicationChanged();
            base.StateHasChanged();
        }

        public async void SelectedApplicationChanged() 
        {
            if (SelectedApplication != null)
            {
                Modules = await ModuleService.GetModules(SelectedApplication.ApplicationId);
                if (Modules.Any())
                {
                    foreach (var m in Modules)
                    {
                        DisplayModules.Add(new Dropdown() { Id = m.ModuleId, Value = m.ModuleName });
                    }
                }
                Roles = await AddRoleService.GetRoles(SelectedApplication.ApplicationId);
                if (Roles.Any())
                {
                    foreach (var r in Roles)
                    {
                        DisplayRoles.Add(new Dropdown() { Id = r.Id, Value = r.RoleName });
                    }
                }
            }
            StateHasChanged();
        }

        public async void ApplicationButtonClick(Models.Applications.Application application)
        {
            await SetSelectedApplication(application);
            base.StateHasChanged();
        }

        public void UpdateApplicationImage()
        {
            foreach (var app in DisplayApplications)
            {
                if (app.LogoImageType.Equals(".svg"))
                {
                    app.ImagePath = "data:image/svg+xml;base64,";
                }
                else if (app.LogoImageType.Equals(".png"))
                {
                    app.ImagePath = "data:image/png;base64,";
                }
                else if (app.LogoImageType.Equals(".jpg"))
                {
                    app.ImagePath = "data:image/jpg;base64,";
                }
                app.ImagePath += System.Convert.ToBase64String(app.LogoImage);
            }
            base.StateHasChanged();
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                application_state = LoadingContainerState.Loading;
                DisplayApplications.Clear();
                DisplayModules.Clear();
                DisplayRoles.Clear();
                Applications = await ApplicationService.GetApplications(commonService.SelectedClient.ClientId);
                if (Applications.Any())
                {
                    foreach (var item in Applications)
                    {
                        DisplayApplications.Add(item);
                    }
                    UpdateApplicationImage();
                    SelectedApplicationChanged();
                    application_state = LoadingContainerState.Loaded;
                }
                else
                {
                    application_state = LoadingContainerState.Loaded;
                }
            }
            catch (Exception e)
            {
                throw e;
            }
        }

    }
}
