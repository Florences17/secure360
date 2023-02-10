using Microsoft.AspNetCore.Components;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Clients;
using System.Net.Http.Json;
using Opteamix.AuthorizationFramework.BlazorApp.Models;

namespace Opteamix.AuthorizationFramework.BlazorApp.Services
{

    public interface IClientService
    {
        public FetchClientModel Clients { get; }

        Task<IEnumerable<FetchUpdateClient>> GetClient(int clientId);

        Task<IEnumerable<FetchClientModel>> GetClients();

        Task<bool> AddClient(AddClientModel client);

        Task<string> DeleteClient(List<int> clientId);

        Task<bool> UpdateClient(AddClientModel client);
        //Task<FetchClientModel> GetClientsPatination(int PageIndex, int PageSize);

    }

    public class ClientService : IClientService
    {
        private HttpClient httpClient;
        private NavigationManager _navigationManager;
        public ClientService(HttpClient _httpClient, NavigationManager navigationManager)
        {
            this.httpClient = _httpClient;
            _navigationManager = navigationManager;
        }

        public FetchClientModel Clients => throw new NotImplementedException();

        public async Task<IEnumerable<FetchUpdateClient>> GetClient(int clientId)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/client?clientId={clientId}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<FetchUpdateClient>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<FetchUpdateClient>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code: {response.StatusCode} message: {message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<IEnumerable<FetchClientModel>> GetClients()
        {
            try
            {
                var response = await httpClient.GetAsync($"api/clients");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<FetchClientModel>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<FetchClientModel>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code: {response.StatusCode} message: {message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> AddClient(AddClientModel client)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<AddClientModel>("api/client/add", client);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return false;
                    }

                    return true;
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} Message: {message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }


        public async Task<string> DeleteClient(List<int> clientId)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<List<int>>("api/client/delete", clientId);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Constants.ContentNotFound;
                    }

                    return await response.Content.ReadAsStringAsync();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} Message: {message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
        }

        public async Task<bool> UpdateClient(AddClientModel client)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync<AddClientModel>("api/client/update", client);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return false;
                    }

                    return true;
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status:{response.StatusCode} Message: {message}");
                }
            }
            catch (Exception)
            {
                throw;
            }
            
        }

    }  
}
