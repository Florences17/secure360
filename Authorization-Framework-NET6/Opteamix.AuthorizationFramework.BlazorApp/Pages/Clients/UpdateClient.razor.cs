using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Clients;
using Opteamix.AuthorizationFramework.BlazorApp.Services;

namespace Opteamix.AuthorizationFramework.BlazorApp.Pages.Clients
{
    public partial class UpdateClient
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

        private int maxAllowedFiles = 3;

        string svgDataURL = string.Empty;

        public enum LoadingContainerState { Loading, Loaded, Error }

        LoadingContainerState client_state;

        public async Task AddImageButtonClick()
        {
            string image_picker_id = "#updateclientimageupload";
            await js.InvokeAsync<Task>("imageselector", image_picker_id);
        }

        public async Task UploadPreviewImage(InputFileChangeEventArgs e)
        {
            var parameters = new Image_properties()
            {
                image_picker_id = "#updateclientimageupload",
                image_preview_id = "#update-client-image",
                image_text_id = "#update-client-text"
            };
            await js.InvokeVoidAsync("uploadpreview", parameters);
            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                try
                {
                    var trustedFileNameForFileStorage = Path.GetRandomFileName();
                    Stream filesteam = new StreamContent(e.File.OpenReadStream(e.File.Size)).ReadAsStream();
                    byte[] fileimage = new byte[filesteam.Length];
                    await filesteam.ReadAsync(fileimage, 0, fileimage.Length);
                    LogoImage = fileimage;
                    LogoImageName = e.File.Name;
                    filesteam.Close();
                    var imageSrc = Convert.ToBase64String(fileimage);
                    LogoImageName = Path.GetFileNameWithoutExtension(e.File.Name); // Get the name only
                    LogoImageType = Path.GetExtension(e.File.Name);
                    svgDataURL = string.Format("data:image;base64,{0}", imageSrc);
                    StateHasChanged();
                }
                catch (Exception ex)
                {
                    throw;
                }
            }
        }

        public async Task ClearPreviewImage()
        {
            var parameters = new Image_properties()
            {
                image_picker_id = "#updateclientimageupload",
                image_preview_id = "#update-client-image",
                image_text_id = "#update-client-text"
            };
            await js.InvokeVoidAsync("clearpreview", parameters);
        }

        public async void SaveButtonClick()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(ShortName)|| string.IsNullOrEmpty(ContactPerson))
            {
                await js.InvokeVoidAsync("alert", "null");
            }
            else 
            {
                AddClientModel addClientModel = new AddClientModel
                {
                    ClientId = commonService.SelectedClient.ClientId,
                    Name = Name,
                    ShortName = ShortName,
                    ClientStatus = 1,
                    IsDeleted = false,
                    ClientLogoImage = LogoImage,
                    ClientLogoName = LogoImageName,
                    ClientLogoImageType = LogoImageType,
                    EmailAddress = Email,
                    WebsiteAddress = Website,
                    AddressLine1 = AddressLine1,
                    AddressLine2 = AddressLine2,
                    City = City,
                    ContactPerson = ContactPerson,
                    ContactPersonEmailAddress = CPEmail,
                    ContactPersonPhoneNumber = CPNumber
                };
                try
                {
                    await ClientService.UpdateClient(addClientModel);
                    navigationManager.NavigateTo(Constants.Client);
                }
                catch (Exception ex)
                {
                    await js.InvokeVoidAsync("alert", ex.ToString());
                }
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
                        Applications = client.Applications;
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
        public async void CancelButtonClick(string url)
        {
            navigationManager.NavigateTo(url);
        }
        private bool CancelDialogOpen { get; set; }
        private async void OpenCancelDialog()
        {
            CancelDialogOpen = true;
        }
        protected async Task OnCancelDialogClose(bool accepted)
        {
            try
            {
                if (accepted)
                {
                    CancelButtonClick(Constants.Client);
                }
                CancelDialogOpen = false;

            }
            catch (Exception ex)
            {

                return;
            }

        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var parameters = new Image_properties()
                {
                    image_picker_id = "#updateclientimageupload",
                    image_preview_id = "#update-client-image",
                    image_text_id = "#update-client-text"
                };

                await js.InvokeVoidAsync("loadimagefromdb", parameters);
            }
            base.OnAfterRenderAsync(firstRender);
        }
    }

    public class Image_propertiess    {
        public string image_picker_id { get; set; }
        public string image_preview_id { get; set; }
        public string image_text_id { get; set; }
    }
}