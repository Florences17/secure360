using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Opteamix.AuthorizationFramework.BlazorApp.Services;

namespace Opteamix.AuthorizationFramework.BlazorApp.Shared
{
    public partial class MenuItems
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        [Inject]
        public CommonService commonService { get; set; }

        [Inject]
        IJSRuntime js { get; set; }

        [Inject]
        public IToastService toastService { get; set; }

        public static string nav_url = "";

        public string NavigationURL
        {
            get
            {
                return nav_url;
            }
            set
            {
                if (value != nav_url)
                {
                    nav_url = value;
                }
            }
        }

        [Parameter]
        public int SelectedApplication { get; set; } 

        public async void MenuButtonClick(string url)
        {
            if (url.Equals(string.Empty))
            {
                toastService.ShowWarning("Select a client to continue");
            }
            else 
            {
                navigationManager.NavigateTo(url);
                OnMenuItemChanged();
            }
        }

        public void OnMenuItemChanged()
        {
            NavigationURL = navigationManager.Uri;
        }

        protected override async Task OnInitializedAsync()
        {
            NavigationURL = navigationManager.Uri;
        }
    }
}
