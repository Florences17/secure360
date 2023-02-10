using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Dropdown;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Modules;
using Opteamix.AuthorizationFramework.BlazorApp.Services;

namespace Opteamix.AuthorizationFramework.BlazorApp.Pages.Modules
{
    public partial class UpdateModule
    {
        public int ModuleId { get; set; }
        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public string Application { get; set; }

        public string Description { get; set; }

        public string ParentModuleSelected { get; set; } = 0.ToString();

        public IEnumerable<Module> ModuleItem { get; set; }

        [Inject]
        IModuleService ModuleService { get; set; }

        [Inject]
        IJSRuntime js { get; set; }

        [Inject]
        public CommonService commonService { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }

        public enum LoadingContainerState { Loading, Loaded, Error }

        LoadingContainerState module_state;

        public IEnumerable<Module> Modules { get; set; }

        public List<Dropdown> ModuleType { get; set; } = new List<Dropdown>();

        public List<Dropdown> ParentModules { get; set; } = new List<Dropdown>();

        public async void SaveButtonClick()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Abbreviation))
            {
                await js.InvokeVoidAsync("alert", "null");
            }
            else
            {
                AddModuleModel addModule = new AddModuleModel
                {
                    Id = ModuleId,
                    Name = Name,
                    Abbreviation = Abbreviation,
                    Description = Description,
                    ParentModule = int.Parse(ParentModuleSelected),
                    ApplicationId = commonService.SelectedApplication.ApplicationId
                };
                try
                {
                    await ModuleService.UpdateModule(addModule);
                    navigationManager.NavigateTo(Constants.Application);
                }
                catch (Exception ex)
                {
                    await js.InvokeVoidAsync("alert", ex.ToString());
                }
            }
        }

        private bool CancelDialogOpen { get; set; }
        private async void OpenCancelDialog()
        {
            CancelDialogOpen = true;
        }
        public async void CancelButtonClick(string url)
        {
            CancelDialogOpen = false;
            navigationManager.NavigateTo(url);
        }
        protected async Task OnCancelDialogClose(bool accepted)
        {
            try
            {
                if (accepted)
                {
                    CancelButtonClick(Constants.Application);
                }
                CancelDialogOpen = false;

            }
            catch (Exception ex)
            {

                return;
            }

        }

        protected override async Task OnInitializedAsync() 
        {
            try
            {
                module_state = LoadingContainerState.Loading;
                ModuleItem = await ModuleService.GetModule(commonService.SelectedModule);
                if (ModuleItem.Any())
                {
                    foreach (var item in ModuleItem)
                    {
                        ModuleId = item.ModuleId;
                        Name = item.ModuleName;
                        Application = item.ApplicationName;
                        Abbreviation = item.Abbreviation;
                        Description = item.Description;
                    }
                }
                Modules = await ModuleService.GetModules(commonService.SelectedApplication.ApplicationId);
                ParentModules.Add(new Dropdown() { Id = 0, Value = "Root" });
                if (Modules.Any())
                {
                    foreach (var m in Modules)
                    {
                        ParentModules.Add(new Dropdown() { Id = m.ModuleId, Value = m.ModuleName });
                    }
                    module_state = LoadingContainerState.Loaded;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }
    }
}
