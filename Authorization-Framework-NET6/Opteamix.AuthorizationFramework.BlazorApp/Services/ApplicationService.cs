using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.AspNetCore.Http;
using Microsoft.JSInterop;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Applications;
using Opteamix.AuthorizationFramework.BlazorApp.Pages.Application;
using System.Net.Http;
using System.Net.Http.Headers;
using System.Net.Http.Json;


namespace Opteamix.AuthorizationFramework.BlazorApp.Services
{
    public interface IApplicationService
    {
        public AddApplicationModel Application { get; }
        Task<IEnumerable<Models.Applications.Application>> GetApplications(int clientid);
        Task<IEnumerable<Models.Applications.Application>> GetApplicationData(int applicationId);
        Task<IEnumerable<Models.Applications.Application>> GetAllApplications();

        Task<bool> ImportData(int ClientId, int ApplicationId, IFormFile File);
        //Task<bool> ExportExcel(int ClientId, int ApplicationId);
        Task<bool> AddApplication(AddApplicationModel application);
        Task<bool> UpdateApplication(AddApplicationModel module);
        Task<string> DeleteApplication(int applicationID);
    }
    public class ApplicationService:IApplicationService
    {
        private HttpClient httpClient;
        private NavigationManager _navigationManager;
        private IJSRuntime js;
        public ApplicationService(HttpClient _httpClient, NavigationManager navigationManager)
        {
            this.httpClient = _httpClient;
            _navigationManager = navigationManager;
        }

        public AddApplicationModel Application => throw new NotImplementedException();

        public async Task<IEnumerable<Models.Applications.Application>> GetApplications (int clientid)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/application?clientid={clientid}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<Models.Applications.Application>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<Models.Applications.Application>>();
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

        public async Task<IEnumerable<Models.Applications.Application>> GetApplicationData(int applicationId)
        {
            try
            {
                var response = await httpClient.GetAsync($"api/application/getApplicationData?applicationId={applicationId}");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<Models.Applications.Application>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<Models.Applications.Application>>();
                }
                else
                {
                    var message = await response.Content.ReadAsStringAsync();
                    throw new Exception($"Http status code: {response.StatusCode} message: {message}");
                }
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        public async Task<IEnumerable<Models.Applications.Application>> GetAllApplications()
        {
            try
            {
                var response = await httpClient.GetAsync($"api/applications");

                if (response.IsSuccessStatusCode)
                {
                    if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
                    {
                        return Enumerable.Empty<Models.Applications.Application>();
                    }
                    return await response.Content.ReadFromJsonAsync<IEnumerable<Models.Applications.Application>>();
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

        //public async Task<bool> ExportExcel (int ClientId, int ApplicationId) 
        //{
        //    using (var content = new MultipartFormDataContent())
        //    {
        //        content.Add(new StringContent(ClientId.ToString()), "ClientId");
        //        content.Add(new StringContent(ApplicationId.ToString()), "ApplicationId");

        //        try
        //        {
        //            var response = await httpClient.PostAsync("api/Excel/Export", content);

        //            if (response.IsSuccessStatusCode)
        //            {
        //                if (response.StatusCode == System.Net.HttpStatusCode.NoContent)
        //                {
        //                    return false;
        //                }
        //                else
        //                {
        //                    return true;

        //                }
        //            }
        //            else
        //            {
        //                var message = await response.Content.ReadAsStringAsync();
        //                throw new Exception($"Http status code: {response.StatusCode} message: {message}");
        //            }
        //        }
        //        catch (Exception)
        //        {
        //            throw;
        //        }
        //    }
        //}

        public async Task<bool> AddApplication(AddApplicationModel application)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<AddApplicationModel>("api/application/add", application);

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

        public async Task<bool> ImportData(int ClientId, int ApplicationId, IFormFile File) 
        {
            using (var content = new MultipartFormDataContent())
            {
                content.Add(new StringContent(ClientId.ToString()),"ClientId");
                content.Add(new StringContent(ApplicationId.ToString()),"ApplicationId");
                content.Add(new StreamContent(File.OpenReadStream())
                {
                    Headers =
                    {
                        ContentLength = File.Length,
                        ContentType = new MediaTypeHeaderValue(File.ContentType)
                    }
                }, "File", File.FileName);

                try
                {
                    var response = await httpClient.PostAsync("api/Excel/Import", content);
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

        public async Task<bool> UpdateApplication(AddApplicationModel applicationmodel)
        {

            try
            {
                var response = await httpClient.PostAsJsonAsync<AddApplicationModel>("api/application/update", applicationmodel);

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
            catch (Exception ex)
            {
                throw;
            }
        }
        public async Task<string> DeleteApplication(int applicationid)
        {
            try
            {
                var response = await httpClient.PostAsJsonAsync<Int32>($"/api/application/delete?applicationid={applicationid}", Convert.ToInt32(applicationid));

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
