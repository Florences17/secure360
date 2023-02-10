using Blazored.Toast.Services;
using Microsoft.AspNetCore.Components;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Clients;
using Opteamix.AuthorizationFramework.BlazorApp.Services;

namespace Opteamix.AuthorizationFramework.BlazorApp.Pages.Clients
{
    public partial class Client
    {
        [Inject]
        public NavigationManager navigationManager { get; set; }

        [Inject]
        IClientService ClientService { get; set; }

        [Inject]
        public CommonService commonService { get; set; }

        [Inject]
        public IToastService toastService { get; set; }

        public IEnumerable<FetchClientModel> Clients { get; set; }

        public List<FetchClientModel> DisplayClients { get; set; } = new List<FetchClientModel>();

        public string SearchValue { get; set; }

        public enum LoadingContainerState { Loading, Loaded, Error }

        LoadingContainerState client_state;

        public List<int> SelectedClient { get; set; } = new List<int>();

        public int CurrentPage { get; set; } = 1;

        public int CurrentIndex { get; set; } = 0;

        public int TotalCount { get; set; }

        public int TotalPages { get; set; } = 1;

        public int PageCount { get; set; } = 1;

        public async void MenuButtonClick(string url)
        {
            navigationManager.NavigateTo(url);
        }

        public async void SearchButtonClicked()
        {
            DisplayClients.Clear();
            foreach (var c in Clients)
            {
                if (c.ClientName.Contains(SearchValue) || c.ShortName == SearchValue)
                {
                    DisplayClients.Add(c);
                }
            }
        }

        public async void GridItemSelected(FetchClientModel model)
        {
            commonService.SetSelectedClient(model);
            navigationManager.NavigateTo(Constants.ClientDetails);
        }

        public void SearchCleared()
        {
            if (string.IsNullOrEmpty(SearchValue))
            {
                DisplayClients.Clear();
                foreach (var c in Clients)
                {
                    DisplayClients.Add(c);
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
                    await ClientService.DeleteClient(SelectedClient);
                    navigationManager.NavigateTo(url, true);
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
        }



        void ClientSelected(int clientID, object checkedValue)
        {
            if ((bool)checkedValue)
            {
                if (!SelectedClient.Contains(clientID))
                {
                    SelectedClient.Add(clientID);
                }
            }
            else
            {
                if (SelectedClient.Contains(clientID))
                {
                    SelectedClient.Remove(clientID);
                }
            }
        }

        public async Task UpdateClientList()
        {
            DisplayClients.Clear();
            try
            {
                Clients = await ClientService.GetClients();
                if (Clients.Any())
                {
                    TotalCount = Clients.Count();
                    TotalPages = Convert.ToInt32(Math.Ceiling(Convert.ToDecimal(TotalCount) / Convert.ToDecimal(PageCount)));
                    if (CurrentIndex == 0)
                    {
                        CurrentPage = 1;
                    }
                    else
                    {
                        CurrentPage = Convert.ToInt32(CurrentIndex / PageCount) + 1;
                    }
                    Clients = Clients.Skip(CurrentIndex).Take(PageCount);
                    foreach (FetchClientModel client in Clients)
                    {
                        DisplayClients.Add(client);
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
            if((Convert.ToInt32(CurrentIndex / PageCount)) > 0) 
            {
                CurrentIndex = CurrentIndex - PageCount;
                await UpdateClientList();
                StateHasChanged();
            }
            else if((Convert.ToInt32(CurrentIndex / PageCount)) == 0) 
            {
                toastService.ShowWarning("No more previous records to be displayed.");
            }
        }

        public async void NextButtonClicked()
        {
            if((Convert.ToInt32(CurrentIndex/PageCount))+1 < (TotalPages))
            {
                CurrentIndex = CurrentIndex + PageCount;
                await UpdateClientList();
                StateHasChanged();
            }
            else if((Convert.ToInt32(CurrentIndex / PageCount))+1 >= (TotalPages))
            {
                toastService.ShowWarning("No more records to be displayed.");
            }
        }

        public async void FirstButtonClicked()
        {
            if((Convert.ToInt32(CurrentIndex / PageCount)) == 0)
            {
                toastService.ShowWarning("No more previous records to be displayed.");
            }
            else 
            {
                CurrentIndex = 0;
                await UpdateClientList();
                StateHasChanged();
            }
        }

        public async void LastButtonClicked()
        {
            if((Convert.ToInt32(CurrentIndex / PageCount)) + 1 >= (TotalPages)) 
            {
                toastService.ShowWarning("No more records to be displayed.");
            }
            else 
            {
                CurrentIndex = CurrentIndex + PageCount;
                if ((Convert.ToInt32(CurrentIndex/PageCount))+1 == TotalPages) 
                {
                    
                    await UpdateClientList();
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
            DisplayClients.Clear();
            try
            {
                client_state = LoadingContainerState.Loading;
                Clients = await ClientService.GetClients();
                if (Clients.Any())
                {
                    TotalCount = Clients.Count();
                    TotalPages = Convert.ToInt32(TotalCount / PageCount);
                    if(CurrentIndex == 0) 
                    {
                        CurrentPage = 1;
                    }
                    else 
                    {
                        CurrentPage = Convert.ToInt32(CurrentIndex / PageCount) + 1;
                    }
                    Clients = Clients.Skip(CurrentIndex).Take(PageCount);
                    foreach (FetchClientModel client in Clients)
                    {
                        DisplayClients.Add(client);
                    }
                    StateHasChanged();
                    client_state = LoadingContainerState.Loaded;
                }
                else
                {
                    client_state = LoadingContainerState.Loaded;
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
