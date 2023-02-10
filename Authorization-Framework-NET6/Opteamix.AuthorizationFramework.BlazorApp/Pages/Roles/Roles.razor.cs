using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Roles;
using Opteamix.AuthorizationFramework.BlazorApp.Services;

namespace Opteamix.AuthorizationFramework.BlazorApp.Pages.Roles
{
    public partial class Roles
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        [Inject]
        IAddRoleService AddRoleService { get; set; }

        [Inject]
        public IToastService toastService { get; set; }

        public IEnumerable<FetchRolesModel>? ApplicationRoles { get; set; }
        //Searcing purpose
        public IEnumerable<FetchRolesModel>? ApplicationRoles1 { get; set; }

        public List<FetchRolesModel> DisplayRoleName { get; set; } = new List<FetchRolesModel>();
        public List<FetchRolesModel> SearchedRoleNamelst { get; set; } = new List<FetchRolesModel>();

        public string? SearchValue { get; set; }

        public enum LoadingContainerState { Loading, Loaded, Error }

        LoadingContainerState Role_State;

        [Inject]
        public CommonService commonService { get; set; }

        public List<int> SelectedRoles { get; set; } = new List<int>();

        public int CurrentPage { get; set; } = 1;

        public int CurrentIndex { get; set; } = 0;

        public int TotalCount { get; set; }

        public int TotalPages { get; set; } = 1;

        public int PageCount { get; set; } = 8;

        public async void GridItemSelected(FetchRolesModel model)
        {
            commonService.SetSelectedRole(model);
            navigationManager.NavigateTo(Constants.RoleDetails);
        }


        public async void SearchButtonClicked1()
        {
            DisplayRoleName.Clear();
            SearchedRoleNamelst.Clear();
            foreach (var c in ApplicationRoles1)
            {
                if (c.RoleName.Contains(SearchValue, StringComparison.CurrentCultureIgnoreCase) || c.ShortName == SearchValue)
                {
                    SearchedRoleNamelst.Add(c);
                }
            }
            CurrentIndex = 0;
            CurrentPage = 1;
            var SearchedRoleNamelstInem = SearchedRoleNamelst.AsEnumerable();
            TotalCount = SearchedRoleNamelstInem.Count();
            TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(TotalCount) / Convert.ToDecimal(PageCount)));
            SearchedRoleNamelstInem = SearchedRoleNamelstInem.Skip(CurrentIndex).Take(PageCount);
            foreach (var c in SearchedRoleNamelstInem)
            {
                DisplayRoleName.Add(c);
            }
            if (string.IsNullOrEmpty(SearchValue))
            {
                UpdateRoleList();
            }

        }

        public async void SearchButtonClicked()
        {
            DisplayRoleName.Clear();
            SearchedRoleNamelst.Clear();
            foreach (var c in ApplicationRoles1)
            {
                if (c.RoleName.Contains(SearchValue, StringComparison.CurrentCultureIgnoreCase) || c.ShortName == SearchValue)
                {
                    SearchedRoleNamelst.Add(c);
                }
            }
            if (CurrentIndex == 0)
            {
                CurrentPage = 1;

            }
            else
            {
                CurrentPage = Convert.ToInt32(CurrentIndex / PageCount) + 1;
            }
            var SearchedRoleNamelstInem = SearchedRoleNamelst.AsEnumerable();
            TotalCount = SearchedRoleNamelstInem.Count();
            TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(TotalCount) / Convert.ToDecimal(PageCount)));
            SearchedRoleNamelstInem = SearchedRoleNamelstInem.Skip(CurrentIndex).Take(PageCount);

            foreach (var c in SearchedRoleNamelstInem)
            {
                DisplayRoleName.Add(c);
            }

            if (string.IsNullOrEmpty(SearchValue))
            {
                UpdateRoleList();
            }
        }
        public void SearchCleared()
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                UpdateRoleList();
            }
        }

        public async void MenuButtonClick(string url)
        {
            navigationManager.NavigateTo(url);
        }

        public async void IconButtonClick(string url, string button_name)
        {
            if (button_name == "create")
            {
                navigationManager.NavigateTo(url);
            }
            else if (button_name == "delete")
            {
                await AddRoleService.DeleteRoles(SelectedRoles);
                DisplayRoleName.Clear();
                await OnInitializedAsync();
            }
        }
        void RoleSelected(int RoleName, object checkedValue)
        {
            if ((bool)checkedValue)
            {
                if (!SelectedRoles.Contains(RoleName))
                {
                    SelectedRoles.Add(RoleName);
                }
            }
            else
            {
                if (SelectedRoles.Contains(RoleName))
                {
                    SelectedRoles.Remove(RoleName);
                }
            }
        }

        public async Task UpdateRoleList()
        {
            DisplayRoleName.Clear();
            try
            {
                ApplicationRoles = await AddRoleService.GetRoles(commonService.SelectedApplication.ApplicationId);
                if (ApplicationRoles.Any())
                {
                    ApplicationRoles = ApplicationRoles.OrderBy(a => a.RoleName);
                    TotalCount = ApplicationRoles.Count();
                    TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(TotalCount) / Convert.ToDecimal(PageCount)));
                    if (CurrentIndex == 0)
                    {
                        CurrentPage = 1;
                    }
                    else
                    {
                        CurrentPage = Convert.ToInt32(CurrentIndex / PageCount) + 1;
                    }
                    ApplicationRoles = ApplicationRoles.Skip(CurrentIndex).Take(PageCount);
                    foreach (FetchRolesModel Roles in ApplicationRoles)
                    {
                        DisplayRoleName.Add(Roles);
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
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    SearchButtonClicked();
                    return;
                }
                await UpdateRoleList();
                StateHasChanged();
            }
            else if ((Convert.ToInt32(CurrentIndex / PageCount)) == 0)
            {
                toastService.ShowWarning("No more previous records to be displayed.");
            }
        }

        public async void NextButtonClicked()
        {

            Console.WriteLine("CurrentIndex In Next=" + CurrentIndex);
            if ((Convert.ToInt32(CurrentIndex / PageCount)) + 1 < (TotalPages))
            {
                CurrentIndex = CurrentIndex + PageCount;
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    SearchButtonClicked();
                    return;
                }
                await UpdateRoleList();
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
                if (!string.IsNullOrEmpty(SearchValue))
                {
                    SearchButtonClicked();
                    return;
                }
                await UpdateRoleList();
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
                    if (!string.IsNullOrEmpty(SearchValue))
                    {
                        SearchButtonClicked();
                        return;
                    }
                    await UpdateRoleList();
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
                Role_State = LoadingContainerState.Loading;
                ApplicationRoles = await AddRoleService.GetRoles(commonService.SelectedApplication.ApplicationId);
                ApplicationRoles1 = await AddRoleService.GetRoles(commonService.SelectedApplication.ApplicationId);
                if (ApplicationRoles.Any())
                {
                    ApplicationRoles = ApplicationRoles.OrderBy(a => a.RoleName);
                    TotalCount = ApplicationRoles.Count();
                    TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(TotalCount) / Convert.ToDecimal(PageCount)));
                    Console.WriteLine("CurrentIndex In Role=" + CurrentIndex);
                    if (CurrentIndex == 0)
                    {
                        CurrentPage = 1;
                    }
                    else
                    {
                        CurrentPage = Convert.ToInt32(CurrentIndex / PageCount) + 1;
                    }
                    ApplicationRoles = ApplicationRoles.Skip(CurrentIndex).Take(PageCount);
                    foreach (FetchRolesModel Roles in ApplicationRoles)
                    {
                        DisplayRoleName.Add(Roles);
                    }
                    Role_State = LoadingContainerState.Loaded;
                }
                else
                {
                    Role_State = LoadingContainerState.Loaded;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}