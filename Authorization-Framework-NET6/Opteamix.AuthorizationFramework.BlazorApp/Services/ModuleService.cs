using Microsoft.AspNetCore.Components;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Modules;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.BlazorApp.Services
{
    public interface IModuleService
    {
        public AddModuleModel Module { get; }

        Task<IEnumerable<Module>> GetModules(int appid);

        Task<IEnumerable<Module>> GetModule(int moduleid);

        Task<IEnumerable<SubModule>> GetSubModules();

        Task<IEnumerable<SubModule>> GetSubModules(int moduleid);

        Task<IEnumerable<SubModule>> GetL2SubModules(int submoduleid);

        Task<string> AddModule(AddModuleModel module);
        Task<string> DeleteModule(List<int> moduleId);

        Task<bool> UpdateModule(AddModuleModel module);
    }

    public class ModuleService : IModuleService
    {
        private HttpClient httpClient;
        private NavigationManager _navigationManager;

        public AddModuleModel Module { get; private set; }

        public ModuleService(HttpClient _httpClient, NavigationManager navigationManager)
        {
            this.httpClient = _httpClient;
            _navigationManager = navigationManager;
        }

        public async Task<IEnumerable<Module>> GetModules(int appid) 
        {
            try
            {
                var response = await httpClient.GetAsync($"api/modules?appid={appid}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<Module>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<Module>>();
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

        public async Task<IEnumerable<Module>> GetModule(int moduleid)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/module?moduleid={moduleid}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return null;
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<Module>>();
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

        public async Task<IEnumerable<SubModule>> GetSubModules()
        {
            try
            {
                var response = await httpClient.GetAsync($"api/allsubmodules");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return null;
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<SubModule>>();
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

        public async Task<IEnumerable<SubModule>> GetSubModules(int moduleid)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/submodules?moduleid={moduleid}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return null;
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<SubModule>>();
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

        public async Task<IEnumerable<SubModule>> GetL2SubModules(int submoduleid)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/l2submodules?submoduleid={submoduleid}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return null;
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<SubModule>>();
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

        public async Task<IEnumerable<Module>> GetSubModule(int submoduleid)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/submodule?submoduleid={submoduleid}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return null;
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<Module>>();
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

        public async Task<string> AddModule(AddModuleModel module)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<AddModuleModel>("api/modules/add", module);

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

        public async Task<bool> UpdateModule(AddModuleModel module)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync<AddModuleModel>("api/modules/update", module);

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

        public async Task<string> DeleteModule(List<int> moduleId)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<List<int>>("api/modules/delete", moduleId);

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