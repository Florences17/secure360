using Microsoft.AspNetCore.Components;
using Opteamix.AuthorizationFramework.BlazorApp.Models.User;
using System.Net.Http.Json;

namespace Opteamix.AuthorizationFramework.BlazorApp.Services
{
    public interface IUserService
    {
        public FetchUserModel Users { get; }
        Task<IEnumerable<FetchUserModel>> GetUsers(int clientid);
    }

    public class UserService: IUserService
    {
        private HttpClient httpClient;
        private NavigationManager _navigationManager;

        public UserService(HttpClient _httpClient, NavigationManager navigationManager)
        {
            this.httpClient = _httpClient;
            _navigationManager = navigationManager;
        }

        public FetchUserModel Users => throw new NotImplementedException();

        public async Task<IEnumerable<FetchUserModel>> GetUsers(int clientid)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/users?clientid={clientid}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<FetchUserModel>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<FetchUserModel>>();
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
    }
}
