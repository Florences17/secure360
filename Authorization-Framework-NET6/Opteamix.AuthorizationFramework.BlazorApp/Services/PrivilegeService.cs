using Microsoft.AspNetCore.Components;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Privilege;

using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.BlazorApp.Services
{
    public interface IPrivilegeService
    {
        // public AddPrivilegeModel Privilege { get; }
        Task<IEnumerable<FetchPrivilegeModel>> GetPrivileges(int appId);
        
        Task<IEnumerable<FetchPrivilegeModel>> GetPrivilege(int privilegeId);

      //  public AddPrivilegeModel Privilege { get; }
        Task<string> AddPrivilege(AddPrivilegeModel Privilege);
        Task<string> DeletePrivilege(List<int> privilegeId);

        Task<bool> UpdatePrivilege(AddPrivilegeModel privilege);
    }
    public class PrivilegeService : IPrivilegeService
    {
        private HttpClient httpClient;
        private NavigationManager _navigationManager;

        // public AddPrivilegeModel Privilege { get; private set; }

        public PrivilegeService(HttpClient _httpClient, NavigationManager navigationManager)
        {
            this.httpClient = _httpClient;
            _navigationManager = navigationManager;
        }


        public async Task<IEnumerable<FetchPrivilegeModel>> GetPrivileges(int appId)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/privileges?appId={appId}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<FetchPrivilegeModel>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<FetchPrivilegeModel>>();
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

         public async Task<IEnumerable<FetchPrivilegeModel>> GetPrivilege(int appid) 
        {
            try
            {
                var response = await httpClient.GetAsync($"api/privilege?privilegeId={appid}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<FetchPrivilegeModel>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<FetchPrivilegeModel>>();
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

        public async Task<string> AddPrivilege(AddPrivilegeModel privilege)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<AddPrivilegeModel>("api/privilege/add", privilege);

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
        
       
        
        public async Task<bool> UpdatePrivilege(AddPrivilegeModel privilege)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync<AddPrivilegeModel>("api/privileges/update", privilege);

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

        public async Task<string> DeletePrivilege(List<int> privilegeId)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<List<int>>("api/privileges/delete", privilegeId);

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
    }
}