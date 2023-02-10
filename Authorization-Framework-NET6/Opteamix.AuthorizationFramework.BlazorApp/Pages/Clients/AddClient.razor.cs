using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Clients;
using Opteamix.AuthorizationFramework.BlazorApp.Services;

namespace Opteamix.AuthorizationFramework.BlazorApp.Pages.Clients
{
    public partial class AddClient
    {
        [Inject]
        IJSRuntime js { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }

        [Inject]
        IClientService ClientService { get; set; }

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

        private int maxAllowedFiles = 3;

        string svgDataURL = string.Empty;

        public async Task AddImageButtonClick()
        {
            string image_picker_id = "#addclientimageupload";
            await js.InvokeAsync<Task>("imageselector", image_picker_id);
        }

        public async Task UploadPreviewImage(InputFileChangeEventArgs e)
        {
            var parameters = new Image_properties()
            {
                image_picker_id = "#addclientimageupload",
                image_preview_id = "#add-client-image",
                image_text_id = "#add-client-text"
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
                    LogoImageName = Path.GetFileNameWithoutExtension(e.File.Name);
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
                image_picker_id = "#addclientimageupload",
                image_preview_id = "#add-client-image",
                image_text_id = "#add-client-text"
            };
            await js.InvokeVoidAsync("clearpreview", parameters);
        }

        public async void SaveButtonClick()
        {
            if (string.IsNullOrEmpty(Name))
            {
                await js.InvokeVoidAsync("alert", "Please enter Client name");
                return;
            }
            if (string.IsNullOrEmpty(ShortName))
            {
                await js.InvokeVoidAsync("alert", "Please enter Short Name");
                return;
            }
            else 
            {
                AddClientModel addClientModel = new AddClientModel
                {
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
                    await ClientService.AddClient (addClientModel);
                    navigationManager.NavigateTo(Constants.Client);
                }
                catch (Exception ex)
                {
                    await js.InvokeVoidAsync("alert", ex.ToString());
                }
            }
        }

        protected override async void OnAfterRender(bool firstRender)
        {
            if (firstRender)
            {
                var parameters = new Image_properties()
                {
                    image_picker_id = "#addclientimageupload",
                    image_preview_id = "#add-client-image",
                    image_text_id = "#add-client-text"
                };
                await js.InvokeVoidAsync("loadfunctions", parameters);
            }

            base.OnAfterRender(firstRender);
        }
        private bool CancelDialogOpen { get; set; }
        private async void OpenCancelDialog()
        {
            CancelDialogOpen = true;
        }
         public async void CancelButtonClick(string url)
        {
            CancelDialogOpen = false;
            navigationManager.NavigateTo(url);
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
    }

    public class Image_properties
    {
        public string image_picker_id { get; set; }
        public string image_preview_id { get; set; }
        public string image_text_id { get; set; }
    }

}