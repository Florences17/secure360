using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Internal;
using Microsoft.JSInterop;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Applications;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Modules;
using Opteamix.AuthorizationFramework.BlazorApp.Services;
using System.Net.Http.Headers;

namespace Opteamix.AuthorizationFramework.BlazorApp.Pages.Application
{
    public partial class Application
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        [Inject]
        IModuleService ModuleService { get; set; }

        [Inject]
        IApplicationService ApplicationService { get; set; }

        [Inject]
        public CommonService commonService { get; set; }

        [Inject]
        public IToastService toastService { get; set; }

        [Inject]
        IJSRuntime js { get; set; }

        public int CurrentPage { get; set; } = 1;

        public int CurrentIndex { get; set; } = 0;

        public int TotalCount { get; set; }

        public int TotalPages { get; set; } = 1;

        public int PageCount { get; set; } = 2;

        public IEnumerable<Module> Modules { get; set; }

        public IEnumerable<SubModule> SubModules { get; set; }

        public IEnumerable<Models.Applications.Application> Applications { get; set; }

        public List<Module> DisplayModules { get; set; } = new List<Module>();

        public List<SubModule> DisplaySubModules { get; set; } = new List<SubModule>();

        public string SearchApplicationValue { get; set; }

        public string SearchModuleValue { get; set; }

        public FormFile ExcelFile { get; set; }

        public enum LoadingContainerState { Loading, Loaded, Error }

        LoadingContainerState application_state;
        LoadingContainerState module_state;

        public List<int> SelectedModules { get; set; } = new List<int>();

        public bool ModuleExpand { get; set; } = false;

        public bool SubModuleExpand { get; set; } = false;

        public void ToggleModuleExpand()
        {
            if (ModuleExpand == true)
            {
                ModuleExpand = false;
            }
            else
            {
                ModuleExpand = true;
            }
        }

        public void ToggleSubModuleExpand()
        {
            if (SubModuleExpand == true)
            {
                SubModuleExpand = false;
            }
            else
            {
                SubModuleExpand = true;
            }
        }

        public async Task HandleExpandStatus(bool status) 
        {        
            ModuleExpand = status;
        }

        public async Task HandleSubExpandStatus(bool status)
        {
            await js.InvokeVoidAsync("alert", "Please Select Application Name");
            SubModuleExpand = status;
        }

        public async void OnSelectedApplicationChanged() 
        {
            module_state = LoadingContainerState.Loading;
            DisplayModules.Clear();
            Modules = await ModuleService.GetModules(commonService.SelectedApplication.ApplicationId);
            if (Modules.Any())
            {
                TotalCount = Modules.Count();
                TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(TotalCount)/ Convert.ToDecimal(PageCount)));
                if (CurrentIndex == 0)
                {
                    CurrentPage = 1;
                }
                else
                {
                    CurrentPage = Convert.ToInt32(CurrentIndex / PageCount) + 1;
                }
                Modules = Modules.Skip(CurrentIndex).Take(PageCount);
                if (Modules.Any())
                {
                    SubModules = await ModuleService.GetSubModules();

                    if (SubModules.Any())
                    {
                        foreach (var submodule in SubModules)
                        {
                            DisplaySubModules.Add(submodule);
                        }
                    }
                }
                foreach (var m in Modules)
                {
                    DisplayModules.Add(m);
                }
            }
            module_state = LoadingContainerState.Loaded;
            base.StateHasChanged();
        }

        public async void ApplicationButtonClick(Models.Applications.Application application )
        {
            commonService.SetSelectedApplication(application);
            OnSelectedApplicationChanged();
        }

        public async void IconButtonClick(string url, string button_name)
        {
            if (button_name == "create")
            {
                navigationManager.NavigateTo(url);
            }
            else if (button_name == "delete")
            {
                try
                {
                    await ModuleService.DeleteModule(SelectedModules);
                    await OnInitializedAsync();
                }
                catch (Exception)
                {
                    throw;
                }
            }
            else if (button_name == "add")
            {
                navigationManager.NavigateTo(url);
            }
            else if (button_name == "update")
            {
                if (commonService.SelectedApplication == null) 
                {
                    toastService.ShowWarning("Select an Application to update");
                }
                else 
                {
                    navigationManager.NavigateTo(url);
                }               
            }
        }

        public async Task OnExcelChanged(InputFileChangeEventArgs e) 
        {
            MemoryStream ms = new MemoryStream();
            await e.File.OpenReadStream().CopyToAsync(ms);
            ExcelFile = new FormFile(ms, 0, ms.Length, "File", e.File.Name)
            {
                Headers = new HeaderDictionary(),
                ContentType = e.File.ContentType
            };
            try
            {
                if(commonService.SelectedApplication==null)
                {
                    await js.InvokeVoidAsync("alert", "Please Select Application Name");
                    return;
                }                
                bool isImport=await ApplicationService.ImportData(commonService.SelectedClient.ClientId, commonService.SelectedApplication.ApplicationId, ExcelFile);
             
                if (isImport)
                {
                    await js.InvokeVoidAsync("alert","Imported Sucessfully");
                }
            }
            catch (Exception ex)
            {
                await js.InvokeVoidAsync("alert", ex.ToString());
            }

        }

        public async void ImageTextButtonClick(string button_name)
        {
           
            if(button_name == "import") 
            {
                string excel_picker_id = "#importexcelfileupload";
                await js.InvokeAsync<Task>("excelselector", excel_picker_id);
            }
            else if(button_name == "export")
            {
                if (commonService.SelectedApplication == null)
                {
                    await js.InvokeVoidAsync("alert", "Please Select Application Name");
                    return;
                }
                string Export_Excel = $"http://secure360revamp.opteamix.com/api/Excel/Export?ClientId={commonService.SelectedClient.ClientId}&ApplicationId={commonService.SelectedApplication.ApplicationId}";
                await js.InvokeAsync<Task>("excelexport", Export_Excel);
            }
        }

        public async void SearchModuleButtonClicked()
        {
            DisplayModules.Clear();
            foreach (var m in Modules)
            {
                if (m.ModuleName.Contains(SearchModuleValue) || m.Abbreviation.Contains(SearchModuleValue) || m.ApplicationName.Contains(SearchModuleValue))
                {
                    DisplayModules.Add(m);
                }
            }
        }

        public async void SearchApplicationButtonClicked() 
        {
            commonService.Applications.Clear();
            foreach (var a in Applications) 
            {
                if (a.ApplicationName.Contains(SearchApplicationValue)) 
                {
                    commonService.SetApplication(a);
                }
            }
        }

        public void SearchModuleCleared()
        {
            if (string.IsNullOrEmpty(SearchModuleValue))
            {
                DisplayModules.Clear();
                foreach (var m in Modules)
                {
                    DisplayModules.Add(m);
                }
            }
        }

        public void SearchApplicationCleared()
        {
            if (string.IsNullOrEmpty(SearchApplicationValue)) 
            {
                commonService.Applications.Clear();
                foreach (var a in Applications) 
                {
                    commonService.SetApplication(a);
                }
            }
        }

        public async void GridItemSelected(int id) 
        {
            commonService.SetSelectedModule(id);
            navigationManager.NavigateTo(Constants.UpdateModule);
        }

        public async void PreviousButtonClicked()
        {
            DisplaySubModules.Clear();
            if ((Convert.ToInt32(CurrentIndex / PageCount)) > 0)
            {
                CurrentIndex = CurrentIndex - PageCount;
                OnSelectedApplicationChanged();
                StateHasChanged();
            }
            else if ((Convert.ToInt32(CurrentIndex / PageCount)) == 0)
            {
                toastService.ShowWarning("No more previous records to be displayed.");
            }
        }

        public async void NextButtonClicked()
        {
            DisplaySubModules.Clear();
            if ((Convert.ToInt32(CurrentIndex / PageCount)) + 1 < (TotalPages))
            {
                CurrentIndex = CurrentIndex + PageCount;
                OnSelectedApplicationChanged();
                StateHasChanged();
            }
            else if ((Convert.ToInt32(CurrentIndex / PageCount)) + 1 >= (TotalPages))
            {
                toastService.ShowWarning("No more records to be displayed.");
            }
        }

        public async void FirstButtonClicked()
        {
            DisplaySubModules.Clear();
            if ((Convert.ToInt32(CurrentIndex / PageCount)) == 0)
            {
                toastService.ShowWarning("No more previous records to be displayed.");
            }
            else
            {
                CurrentIndex = 0;
                OnSelectedApplicationChanged();
                StateHasChanged();
            }
        }

        public async void LastButtonClicked()
        {
            DisplaySubModules.Clear();
            if ((Convert.ToInt32(CurrentIndex / PageCount)) + 1 >= (TotalPages))
            {
                toastService.ShowWarning("No more records to be displayed.");
            }
            else
            {
                CurrentIndex = CurrentIndex + PageCount;
                if ((Convert.ToInt32(CurrentIndex / PageCount)) + 1 == TotalPages)
                {
                    OnSelectedApplicationChanged();
                    StateHasChanged();
                }
                else
                {
                    LastButtonClicked();
                }
            }
        }

        async Task ModulesSelectedAsync(int moduleID, object checkedValue)
        {
            if ((bool)checkedValue)
            {
                if (!SelectedModules.Contains(moduleID))
                {
                    SelectedModules.Add(moduleID);
                }
            }
            else
            {
                if (SelectedModules.Contains(moduleID))
                {
                    SelectedModules.Remove(moduleID);
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                application_state = LoadingContainerState.Loading;
                commonService.Applications.Clear();
                Applications = await ApplicationService.GetApplications(commonService.SelectedClient.ClientId);
                if (Applications.Any())
                {
                    foreach (var item in Applications)
                    {
                        commonService.SetApplication(item);
                    }
                    commonService.UpdateApplicationImage();
                    if (commonService.SelectedApplication != null)
                    {
                        OnSelectedApplicationChanged();
                    }
                    application_state = LoadingContainerState.Loaded;
                }
                else 
                {
                    application_state = LoadingContainerState.Loaded;
                }
            }
            catch (Exception e)
            {
                Console.WriteLine(e);
                throw e;
            }
        }
    }
}