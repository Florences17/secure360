using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Clients;
using Opteamix.AuthorizationFramework.BlazorApp.Services;

namespace Opteamix.AuthorizationFramework.BlazorApp.Pages.Clients
{
    public partial class ClientDetails
    {
        [Inject]
        public CommonService commonService { get; set; }

        [Inject]
        IJSRuntime js { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }

        [Inject]
        IClientService ClientService { get; set; }

        public IEnumerable<FetchUpdateClient> ClientItem { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string Email { get; set; }

        public string Website { get; set; }

        public string AddressLine1 { get; set; }

        public string AddressLine2 { get; set; }

        public string ContactPerson { get; set; }

        public string CPNumber { get; set; }

        public string City { get; set; }

        public string CPEmail { get; set; }

        public byte[]? LogoImage { get; set; }

        public string? LogoImageName { get; set; }

        public string? LogoImageType { get; set; }

        public List<Models.Applications.Application>? Applications { get; set; } = new List<Models.Applications.Application>();

        public string ImagePath { get; set; }

        public enum LoadingContainerState { Loading, Loaded, Error }

        LoadingContainerState client_state;

        public async Task EditImageButtonClick()
        {
            navigationManager.NavigateTo(Constants.UpdateClient);
        }

        public async void ApplicationButtonClick(Models.Applications.Application application)
        {
            commonService.SetSelectedApplication(application);
            navigationManager.NavigateTo(Constants.Application);
        }

        public async void IconButtonClick(string url, string button_name)
        {
            if (button_name == "add")
            {
                navigationManager.NavigateTo(url);
            }
        }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                client_state = LoadingContainerState.Loading;
                ClientItem = await ClientService.GetClient(commonService.SelectedClient.ClientId);
                if (ClientItem.Any())
                {
                    foreach (var client in ClientItem)
                    {
                        Name = client.ClientName;
                        ShortName = client.ShortName;
                        Website = client.WebsiteAddress;
                        Email = client.EmailAddress;
                        AddressLine1 = client.AddressLine1;
                        AddressLine2 = client.AddressLine2;
                        City = client.City;
                        ContactPerson = client.ContactPerson;
                        CPEmail = client.ContactPersonEmailAddress;
                        CPNumber = client.ContactPersonPhoneNumber;
                        LogoImage = client.ClientLogoImage;
                        LogoImageName = client.ClientLogoName;
                        LogoImageType = client.ClientLogoImageType;
                        if (client.ClientLogoImageType.Equals(".svg"))
                        {
                            ImagePath = "data:image/svg+xml;base64,";
                        }
                        else if (client.ClientLogoImageType.Equals(".png"))
                        {
                            ImagePath = "data:image/png;base64,";
                        }
                        else if (client.ClientLogoImageType.Equals(".jpg"))
                        {
                            ImagePath = "data:image/jpg;base64,";
                        }
                        ImagePath += System.Convert.ToBase64String(client.ClientLogoImage);

                        if(client.Applications.Count() > 0) 
                        {
                            foreach (var application in client.Applications)
                            {
                                if (application.LogoImage != null && application.LogoImageType != null)
                                {
                                    if (application.LogoImageType.Equals(".svg"))
                                    {
                                        application.ImagePath = "data:image/svg+xml;base64,";
                                    }
                                    else if (application.LogoImageType.Equals(".png"))
                                    {
                                        application.ImagePath = "data:image/png;base64,";
                                    }
                                    else if (application.LogoImageType.Equals(".jpg"))
                                    {
                                        application.ImagePath = "data:image/jpg;base64,";
                                    }
                                    application.ImagePath += System.Convert.ToBase64String(application.LogoImage);
                                }
                                else
                                {
                                    application.ImagePath = "";
                                }
                                Applications.Add(application);
                            }
                        }
                    }
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
