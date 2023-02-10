using Microsoft.AspNetCore.Components;
using System.Threading.Tasks;
using Blazored.Toast.Services;
using Microsoft.JSInterop;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Privilege;
using Opteamix.AuthorizationFramework.BlazorApp.Services;

namespace Opteamix.AuthorizationFramework.BlazorApp.Pages.Privilege
{
    public partial class AddPrivilege
    {
        public string PrivilegeName { get; set; }

        public string Application { get; set; }

        public string Description { get; set; }

        [Inject]
        IPrivilegeService PrivilegeService { get; set; }

        [Inject]
        CommonService commonService { get; set; }

        [Inject]
        IJSRuntime js { get; set; }
        
        [Inject]
        public IToastService toastService { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }

        

        public async void SaveButtonClick()
        {
            if (string.IsNullOrEmpty(PrivilegeName))
            {
                //await js.InvokeVoidAsync("alert", "null");
                    toastService.ShowError("Cannot insert Null values");
            }
            else
            {
                AddPrivilegeModel AddPrivilege = new AddPrivilegeModel
                {
                    PrivilegeName = PrivilegeName,
                    Description = Description,
                    ApplicationId = commonService.SelectedApplication.ApplicationId
                };
                try
                {
                    await PrivilegeService.AddPrivilege(AddPrivilege);
                    navigationManager.NavigateTo(Constants.Privileges);
                    toastService.ShowSuccess("Privilege Added Successfully");
                }
                catch (Exception ex)
                {
                    // await js.InvokeVoidAsync("alert", ex.ToString());
                    toastService.ShowError("Error occured while inserting the value");
                }
            }
        }


        public async void MenuButtonClick(string url)
        {
            navigationManager.NavigateTo(url);
        }
        public async void CancelButtonClick(string url)
        {
            CancelDialogOpen = false;
            navigationManager.NavigateTo(url);
        }
        private bool CancelDialogOpen { get; set; }
        private async void OpenCancelDialog()
        {

            CancelDialogOpen = true;

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
