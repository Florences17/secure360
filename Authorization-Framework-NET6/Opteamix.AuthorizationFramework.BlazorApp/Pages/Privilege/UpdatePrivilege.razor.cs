using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Blazored.Toast.Services;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Privilege;
using Opteamix.AuthorizationFramework.BlazorApp.Services;

namespace Opteamix.AuthorizationFramework.BlazorApp.Pages.Privilege
{
    public partial class UpdatePrivilege
    {
        public int PrivilegeId { get; set; }
        public string PrivilegeName { get; set; }

        public string Description { get; set; } 
        public string OldPrivilegeName { get; set; }

        public string OldDescription { get; set; }

        public IEnumerable<Privilege> PrivilegeItem { get; set; }

        [Inject]
        IPrivilegeService PrivilegeService { get; set; }

        [Inject]
        IJSRuntime js { get; set; }
        
        [Inject]
        public IToastService toastService { get; set; }

        [Inject]
        public CommonService commonService { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }

        public enum LoadingContainerState { Loading, Loaded, Error }

        LoadingContainerState privilege_state;

        public async void SaveButtonClick()
        {
            if (string.IsNullOrEmpty(PrivilegeName) || string.IsNullOrEmpty(Description))
            {
                await js.InvokeVoidAsync("alert", "null");
                    toastService.ShowError("Cannot insert Null values");
            }
            else
            {
                AddPrivilegeModel addPrivilege = new AddPrivilegeModel
                {
                    Id = PrivilegeId,
                    PrivilegeName = PrivilegeName,
                    Description = Description
                };
                try
                {
                    await PrivilegeService.UpdatePrivilege(addPrivilege);
                    navigationManager.NavigateTo(Constants.Privileges);
                    toastService.ShowSuccess("Privilege updated Successfully");
                }
                catch (Exception ex)
                {
                    await js.InvokeVoidAsync("alert", ex.ToString());
                    toastService.ShowError("Error occured while inserting the value");
                }
            }
        }

        public async void CancelButtonClick(string url)
        {
            Console.WriteLine("OldPrivilegeName=" + OldPrivilegeName + " OldDescription="+ OldDescription);
           
            if (PrivilegeName == OldPrivilegeName && Description == OldDescription)
            {
                CancelDialogOpen = false;
                navigationManager.NavigateTo(url);
            }
            else
            {               
                CancelDialogOpen = true;
                Console.WriteLine("PrivilegeName=" + PrivilegeName + " Description=" + Description);
                Console.WriteLine("CancelDialogOpen=" + CancelDialogOpen);
            }
        }

        protected override async Task OnInitializedAsync() 
        {
            try
            {
                privilege_state = LoadingContainerState.Loading;
                var PrivilegeItem = await PrivilegeService.GetPrivilege(commonService.SelectedPrivilege);
                if (PrivilegeItem.Any())
                {
                    foreach (var item in PrivilegeItem)
                    {
                        PrivilegeId = item.PrivilegeId;
                        OldDescription= item.Description;
                        OldPrivilegeName= item.PrivilegeName;
                        PrivilegeName = item.PrivilegeName;
                        Description=item.Description;
                    }
                    privilege_state = LoadingContainerState.Loaded;
                }
                privilege_state = LoadingContainerState.Loaded;
            }
            catch (Exception e)
            {
                throw;
            }
        }
         public async void DeleteButtonClick()
        {
            PrivilegeName = "";
            Description = "";
        }
        private bool DeleteDialogOpen { get; set; }

        private async void OpenDeleteDialog()
        {
            DeleteDialogOpen = true;
        }
        private bool CancelDialogOpen { get; set; }


      
        private async void OpenCancelDialog(string url)
        {
            if(PrivilegeName==OldPrivilegeName || Description==OldDescription)
            {
                CancelDialogOpen = false;
                navigationManager.NavigateTo(url);
            }
            else
            {
                CancelDialogOpen = true;
            }
            
        }
        protected async Task OnDeleteDialogClose(bool accepted)
        {
            try
            {
                if (accepted)
                {
                    
                   // await PrivilegeService.DeletePrivilege(commonService.SelectedPrivilege.Id);
                    navigationManager.NavigateTo(Constants.Privileges);
                    StateHasChanged();
                }
                DeleteDialogOpen = false;
            }
            catch (Exception ex)
            {
                return;
            }
        }

         protected async Task OnCancelDialogClose(bool accepted)
        {
            try
            {
                if (accepted)
                {
                    CancelButtonClick(Constants.Privileges);
                }
                CancelDialogOpen = false;

            }
            catch (Exception ex)
            {

                return;
            }

        }
    }
}
