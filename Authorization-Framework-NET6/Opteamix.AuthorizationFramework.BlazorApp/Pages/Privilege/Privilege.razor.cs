using Microsoft.AspNetCore.Components;
using Blazored.Toast.Services;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Privilege;
using Opteamix.AuthorizationFramework.BlazorApp.Services;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Blazored.Toast.Services;

namespace Opteamix.AuthorizationFramework.BlazorApp.Pages.Privilege
{
    public partial class Privilege
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        [Inject]
        IPrivilegeService PrivilegeService { get; set; }

        public IEnumerable<FetchPrivilegeModel> Privileges { get; set; }

        public List<FetchPrivilegeModel> DisplayPrivileges { get; set; } = new List<FetchPrivilegeModel>();

        public string SearchValue { get; set; }
        
        [Inject]
        public IToastService toastService { get; set; }

        [Inject]
        public CommonService commonService { get; set; }

        public enum LoadingContainerState { Loading, Loaded, Error }
        public List<int> SelectedPrivileges { get; set; } = new List<int>();

        public MetaData MetaData { get; set; } = new();
        LoadingContainerState privilege_state;

        public int CurrentPage { get; set; } = 1;

        public int CurrentIndex { get; set; } = 0;

        public int TotalCount { get; set; }

        public int TotalPages { get; set; } = 1;

        public int PageCount { get; set; } = 8;

        public async void SearchButtonClicked()
        {
            DisplayPrivileges.Clear();
            foreach (var c in Privileges)
            {
                if (c.PrivilegeName.Contains(SearchValue) || c.Description == SearchValue)
                {
                    DisplayPrivileges.Add(c);
                }
            }
        }

        public void SearchCleared()
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                DisplayPrivileges.Clear();
                foreach (var c in Privileges)
                {
                    DisplayPrivileges.Add(c);
                }
            }
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
                    await PrivilegeService.DeletePrivilege(SelectedPrivileges);
                    DisplayPrivileges.Clear();
                    await OnInitializedAsync();
                    toastService.ShowSuccess("Privilege deleted Successfully");
                }
                catch (Exception)
                {
                    toastService.ShowError("Error occured while deleting the value");
                    throw;
                }
            }
            else if (button_name == "add")
            {
                navigationManager.NavigateTo(url);
            }
        }

        public async void GridItemSelected(int id) 
        {
            commonService.SetSelectedPrivilege(id);
            navigationManager.NavigateTo(Constants.UpdatePrivilege);
        }
        void PrivilegeSelected(int privilegeID, object checkedValue)
        {
            if ((bool)checkedValue)
            {
                if (!SelectedPrivileges.Contains(privilegeID))
                {
                    SelectedPrivileges.Add(privilegeID);
                }
            }
            else
            {
                if (SelectedPrivileges.Contains(privilegeID))
                {
                    SelectedPrivileges.Remove(privilegeID);
                }
            }
        }

        public async Task UpdatePrivilegeList()
        {
            DisplayPrivileges.Clear();
            try
            {
                Privileges = await PrivilegeService.GetPrivileges(commonService.SelectedApplication.ApplicationId);
                if (Privileges.Any())
                {
                    TotalCount = Privileges.Count();
                    TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(TotalCount) / Convert.ToDecimal(PageCount)));
                    if (CurrentIndex == 0)
                    {
                        CurrentPage = 1;
                    }
                    else
                    {
                        CurrentPage = Convert.ToInt32(CurrentIndex / PageCount) + 1;
                    }
                    Privileges = Privileges.Skip(CurrentIndex).Take(PageCount);
                    foreach (FetchPrivilegeModel privilege in Privileges)
                    {
                        DisplayPrivileges.Add(privilege);
                    }
                    StateHasChanged();
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async void PreviousButtonClicked()
        {
            if ((Convert.ToInt32(CurrentIndex / PageCount)) > 0)
            {
                CurrentIndex = CurrentIndex - PageCount;
                await UpdatePrivilegeList();
                StateHasChanged();
            }
            else if ((Convert.ToInt32(CurrentIndex / PageCount)) == 0)
            {
                toastService.ShowWarning("No more previous records to be displayed.");
            }
        }

        public async void NextButtonClicked()
        {
            if ((Convert.ToInt32(CurrentIndex / PageCount)) + 1 < (TotalPages))
            {
                CurrentIndex = CurrentIndex + PageCount;
                await UpdatePrivilegeList();
                StateHasChanged();
            }
            else if ((Convert.ToInt32(CurrentIndex / PageCount)) + 1 >= (TotalPages))
            {
                toastService.ShowWarning("No more records to be displayed.");
            }
        }

        public async void FirstButtonClicked()
        {
            if ((Convert.ToInt32(CurrentIndex / PageCount)) == 0)
            {
                toastService.ShowWarning("No more previous records to be displayed.");
            }
            else
            {
                CurrentIndex = 0;
                await UpdatePrivilegeList();
                StateHasChanged();
            }
        }

        public async void LastButtonClicked()
        {
            if ((Convert.ToInt32(CurrentIndex / PageCount)) + 1 >= (TotalPages))
            {
                toastService.ShowWarning("No more records to be displayed.");
            }
            else
            {
                CurrentIndex = CurrentIndex + PageCount;
                if ((Convert.ToInt32(CurrentIndex / PageCount)) + 1 == TotalPages)
                {

                    await UpdatePrivilegeList();
                    StateHasChanged();
                }
                else
                {
                    LastButtonClicked();
                }
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                privilege_state = LoadingContainerState.Loading;
                Privileges = await PrivilegeService.GetPrivileges(commonService.SelectedApplication.ApplicationId);
                if (Privileges.Any())
                {
                    TotalCount = Privileges.Count();
                    TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(TotalCount) / Convert.ToDecimal(PageCount)));
                    if (CurrentIndex == 0)
                    {
                        CurrentPage = 1;
                    }
                    else
                    {
                        CurrentPage = Convert.ToInt32(CurrentIndex / PageCount) + 1;
                    }
                    Privileges = Privileges.Skip(CurrentIndex).Take(PageCount);
                    foreach (FetchPrivilegeModel privilege in Privileges)
                    {
                        DisplayPrivileges.Add(privilege);
                    }
                    privilege_state = LoadingContainerState.Loaded;
                }
                else
                {
                    privilege_state = LoadingContainerState.Loaded;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        
    }
}
