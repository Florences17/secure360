using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Dropdown;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Modules;
using Opteamix.AuthorizationFramework.BlazorApp.Services;

namespace Opteamix.AuthorizationFramework.BlazorApp.Pages.Modules
{
    public partial class AddModule
    {

        public string Name { get; set; }

        public string Abbreviation { get; set; }

        public int ModuleTypeSelected { get; set; }

        public string ParentModuleSelected { get; set; } = 0.ToString();

        public string Description { get; set; }

        public IEnumerable<Module> Modules { get; set; }
        
        [Inject]
        IModuleService ModuleService { get; set; }

        [Inject]
        CommonService commonService { get; set; }

        [Inject]
        IJSRuntime js { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }

        public List<Dropdown> ParentModules { get; set; } = new List<Dropdown>();

        public async void SaveButtonClick()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(ModuleTypeSelected.ToString()))
            {
                await js.InvokeVoidAsync("alert", "null");
            }
            else
            {
                AddModuleModel addModule = new AddModuleModel
                {
                    Name = Name,
                    Abbreviation = Abbreviation,
                    Description = Description,
                    ParentModule = int.Parse(ParentModuleSelected),
                    ApplicationId = commonService.SelectedApplication.ApplicationId
                };
                try
                {
                    await ModuleService.AddModule(addModule);
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
            Modules = await ModuleService.GetModules(commonService.SelectedApplication.ApplicationId);
            ParentModules.Add(new Dropdown() { Id =0, Value = "Root" });
            if (Modules.Any())
            {
                foreach (var m in Modules)
                {
                    ParentModules.Add(new Dropdown() { Id = m.ModuleId, Value = m.ModuleName });
                }
            }
        }
    }
}
