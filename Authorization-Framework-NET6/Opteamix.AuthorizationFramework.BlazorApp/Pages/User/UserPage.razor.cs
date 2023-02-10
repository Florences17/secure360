using Microsoft.AspNetCore.Components;
using Opteamix.AuthorizationFramework.BlazorApp.Models.User;
using Opteamix.AuthorizationFramework.BlazorApp.Services;

namespace Opteamix.AuthorizationFramework.BlazorApp.Pages.User
{
    public partial class UserPage
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        [Inject]
        IUserService UserService { get; set; }

        [Inject]
        public CommonService commonService { get; set; }

        public IEnumerable<FetchUserModel> FetchUserModels { get; set; }

        public List<FetchUserModel> DisplayUsers { get; set; } = new List<FetchUserModel>();

        public enum LoadingContainerState { Loading, Loaded, Error }

        LoadingContainerState user_state;

        protected override async Task OnInitializedAsync()
        {
            DisplayUsers.Clear();
            try
            {
                user_state = LoadingContainerState.Loading;
                FetchUserModels = await UserService.GetUsers(commonService.SelectedClient.ClientId);
                if (FetchUserModels.Any())
                {
                    foreach (FetchUserModel user in FetchUserModels)
                    {
                        DisplayUsers.Add(user);
                    }
                    user_state = LoadingContainerState.Loaded;
                }
                else 
                {
                    user_state = LoadingContainerState.Loaded;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
    }
}
