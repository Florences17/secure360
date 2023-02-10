using Microsoft.AspNetCore.Components;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Roles;

using System.Net.Http;
using System.Net.Http.Json;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.BlazorApp.Services
{
    public interface IAddRoleService
    {
        public AddRoleModel Role { get; }
        Task<string> AddRole(AddRoleModel role);
        Task<IEnumerable<FetchRolesModel>> GetRoles(int appid);
        Task<string> DeleteRoles(List<int> roleId);
        Task<IEnumerable<FetchRolesModel>> GetRoleId(AddRoleModel role);
        Task<bool> UpdateRole(AddRoleModel role);
        Task<string> AddModulePrivilege(ModulePrivilege addModulePrivilege);
        Task<string> AddRoleModule(RoleModule rolemodule);

        Task<IEnumerable<ModulePrivilege>> GetModPrevId(ModulePrivilege addModulePrivilege);

        Task<IEnumerable<RoleEntity>> GetRoleEntityByRoleId(int roleId);

        Task<IEnumerable<EntityPrivilege>> GetEntityPrivilegeById(int id);
    }

    public class AddRoleService : IAddRoleService
    {
        private HttpClient httpClient;
        private NavigationManager _navigationManager;
        public AddRoleModel Role { get; private set; }

        public AddRoleService(HttpClient _httpClient, NavigationManager navigationManager)
        {
            this.httpClient = _httpClient;
            _navigationManager = navigationManager;
        }

        public async Task<string> AddRole(AddRoleModel role)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<AddRoleModel>("api/roles/add", role);

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

        public async Task<bool> UpdateRole(AddRoleModel role)
        {
            try
            {
                var response = await httpClient.PutAsJsonAsync<AddRoleModel>("api/Roles/update", role);

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

        public async Task<IEnumerable<FetchRolesModel>> GetRoles(int appid)
        {
            try
            {
                var response = await httpClient.GetAsync($"/api/Roles?appid={appid}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<FetchRolesModel>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<FetchRolesModel>>();
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

        public async Task<string> DeleteRoles(List<int> roleId)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<List<int>>("api/Roles/delete", roleId);

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
        public async Task<IEnumerable<FetchRolesModel>> GetRoleId(AddRoleModel role)
        {
            try
            {

                var response = await httpClient.PostAsJsonAsync<AddRoleModel>($"/api/RoleId/get", role);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<FetchRolesModel>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<FetchRolesModel>>();
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

        public async Task<string> AddModulePrivilege(ModulePrivilege addModulePrivilege)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<ModulePrivilege>("api/roles/addModPriv", addModulePrivilege);

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
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<string> AddRoleModule(RoleModule addRoleModule)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<RoleModule>("api/roles/addRoleModule", addRoleModule);

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
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<ModulePrivilege>> GetModPrevId(ModulePrivilege addModulePrivilege)
        {
            try
            {

                var response = await httpClient.PostAsJsonAsync<ModulePrivilege>($"/api/ModPrevId/get", addModulePrivilege);

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<ModulePrivilege>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<ModulePrivilege>>();
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

        public async Task<IEnumerable<RoleEntity>> GetRoleEntityByRoleId(int roleId)
        {
            try
            {

                // var response = await httpClient.PostAsJsonAsync<RoleEntity>($"/api/RoleEntityByRoleId/get", roleId);
                var response = await httpClient.GetAsync($"/api/RoleEntityByRoleId?roleId={roleId}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<RoleEntity>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<RoleEntity>>();
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

        public async Task<IEnumerable<EntityPrivilege>> GetEntityPrivilegeById(int id)
        {
            try
            {

                // var response = await httpClient.PostAsJsonAsync<RoleEntity>($"/api/RoleEntityByRoleId/get", roleId);
                var response = await httpClient.GetAsync($"/api/GetEntityPrivilegeById?id={id}");
                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<EntityPrivilege>();
                    }

                    return await response.Content.ReadFromJsonAsync<IEnumerable<EntityPrivilege>>();
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