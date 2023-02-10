using Microsoft.AspNetCore.Components;
using Microsoft.AspNetCore.Components.Forms;
using Microsoft.JSInterop;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Applications;
using Opteamix.AuthorizationFramework.BlazorApp.Services;

namespace Opteamix.AuthorizationFramework.BlazorApp.Pages.Application
{
    public partial class UpdateApplication
    {
        public string? Name { get; set; }

        public string? Code { get; set; }
        public int ClientId { get; set; }
        public string? Description { get; set; }
        public string Abbreviation { get; set; }
        public byte[]? LogoImage { get; set; }
        public string? LogoImageName { get; set; }
        public string? LogoImageType { get; set; }
        public int? ModifyBy { get; set; }
        public DateTime Modifydate { get; set; }
        public int Id { get; set; }

        public string ImagePath { get; set; }


        public IEnumerable<Application> ApplicationItem { get; set; }
        [Inject]
        IApplicationService ApplicationService { get; set; }

        [Inject]
        IJSRuntime js { get; set; }

        [Inject]
        public CommonService commonService { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }

        public enum LoadingContainerState { Loading, Loaded, Error }
        public async Task ClearPreviewImage()
        {
            var parameters = new Image_properties()
            {
                image_picker_id = "#updateapplicationimageupload",
                image_preview_id = "#update-application-image",
                image_text_id = "#update-application-text"
            };
            await js.InvokeVoidAsync("clearpreview", parameters);
        }

        public async void CancelButtonClick(string url)
        {
            navigationManager.NavigateTo(url);
        }
        public async void UpdateButtonClick()
        {
            if (string.IsNullOrEmpty(Name) || string.IsNullOrEmpty(Abbreviation))
            {
                await js.InvokeVoidAsync("alert", "null");
            }
            else
            {
                AddApplicationModel addApplicationModel = new AddApplicationModel
                {
                    Name = Name,
                    ClientId= ClientId,
                    Code = Code,
                    Description = Description,
                    Abbreviation = Abbreviation,
                    LogoImage = LogoImage,
                    LogoImageName = LogoImageName,
                    LogoImageType = LogoImageType,
                    Id = Id
                };
                try
                {
                    await ApplicationService.UpdateApplication(addApplicationModel);
                    navigationManager.NavigateTo(Constants.Application);
                }
                catch (Exception ex)
                {
                    await js.InvokeVoidAsync("alert", ex.ToString());
                }
            }
        }

        public async void DeleteButtonClick()
        {
            Name = "";
            Code = "";
            Description = "";
            Abbreviation = "";
            LogoImage = null;
            LogoImageName = "";
            LogoImageType = "";
        }
        private bool DeleteDialogOpen { get; set; }

        private async void OpenDeleteDialog()
        {
            DeleteDialogOpen = true;
        }
        private bool CancelDialogOpen { get; set; }
        private async void OpenCancelDialog()
        {
            CancelDialogOpen = true;
        }
        protected async Task OnDeleteDialogClose(bool accepted)
        {
            try
            {
                if (accepted)
                {
                    
                    await ApplicationService.DeleteApplication(commonService.SelectedApplication.ApplicationId);
                    navigationManager.NavigateTo(Constants.Application);
                    StateHasChanged();
                }
                DeleteDialogOpen = false;
            }
            catch (Exception ex)
            {
                return;
            }
        }
        public async Task UploadPreviewImage(InputFileChangeEventArgs e)
        {

            var parameters = new Image_properties()
            {
                image_picker_id = "#updateapplicationimageupload",
                image_preview_id = "#update-application-image",
                image_text_id = "#update-application-text"
            };
            await js.InvokeVoidAsync("uploadpreview", parameters);
            foreach (var file in e.GetMultipleFiles(maxAllowedFiles))
            {
                try
                {
                    // loadedFiles.Add(file);

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
        private List<IBrowserFile> loadedFiles = new();
        private long maxFileSize = 1024 * 15;
        private int maxAllowedFiles = 1;
        string loaderUrl = "data:image/svg+xml;base64,PCEtLSBCeSBTYW0gSGVyYmVydCAoQHNoZXJiKSwgZm9yIGV2ZXJ5b25lLiBNb3JlIEAgaHR0cDovL2dvby5nbC83QUp6YkwgLS0+Cjxzdmcgd2lkdGg9IjM4IiBoZWlnaHQ9IjM4IiB2aWV3Qm94PSIwIDAgMzggMzgiIHhtbG5zPSJodHRwOi8vd3d3LnczLm9yZy8yMDAwL3N2ZyIgc3Ryb2tlPSIjZmZmIj4KICAgIDxnIGZpbGw9Im5vbmUiIGZpbGwtcnVsZT0iZXZlbm9kZCI+CiAgICAgICAgPGcgdHJhbnNmb3JtPSJ0cmFuc2xhdGUoMSAxKSIgc3Ryb2tlLXdpZHRoPSIyIj4KICAgICAgICAgICAgPGNpcmNsZSBzdHJva2Utb3BhY2l0eT0iLjUiIGN4PSIxOCIgY3k9IjE4IiByPSIxOCIvPgogICAgICAgICAgICA8cGF0aCBkPSJNMzYgMThjMC05Ljk0LTguMDYtMTgtMTgtMTgiPgogICAgICAgICAgICAgICAgPGFuaW1hdGVUcmFuc2Zvcm0KICAgICAgICAgICAgICAgICAgICBhdHRyaWJ1dGVOYW1lPSJ0cmFuc2Zvcm0iCiAgICAgICAgICAgICAgICAgICAgdHlwZT0icm90YXRlIgogICAgICAgICAgICAgICAgICAgIGZyb209IjAgMTggMTgiCiAgICAgICAgICAgICAgICAgICAgdG89IjM2MCAxOCAxOCIKICAgICAgICAgICAgICAgICAgICBkdXI9IjFzIgogICAgICAgICAgICAgICAgICAgIHJlcGVhdENvdW50PSJpbmRlZmluaXRlIi8+CiAgICAgICAgICAgIDwvcGF0aD4KICAgICAgICA8L2c+CiAgICA8L2c+Cjwvc3ZnPg==";

        string svgDataURL = string.Empty;
        public static readonly List<string> ImageExtensions = new List<string> { ".JPG", ".JPEG", ".JPE", ".BMP", ".GIF", ".PNG" };

        public async Task AddImageButtonClick()
        {
            string image_picker_id = "#updateapplicationimageupload";
            await js.InvokeAsync<Task>("imageselector", image_picker_id);
        }
        protected async Task OnCancelDialogClose(bool accepted)
        {
            try
            {
                if (accepted)
                {
                    CancelButtonClick(Constants.Application);
                }
                CancelDialogOpen = false;

            }
            catch (Exception ex)
            {

                return;
            }

        }
        LoadingContainerState application_state;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                application_state = LoadingContainerState.Loading;
                var ApplicationItem = await ApplicationService.GetApplicationData(commonService.SelectedApplication.ApplicationId);
                if (ApplicationItem.Any())
                {
                    foreach (var item in ApplicationItem)
                    {
                        Name = item.ApplicationName;
                        Abbreviation = item.Abbreviation;
                        Code = item?.Code;
                        Description = item?.Description;
                        LogoImage=item?.LogoImage;
                        LogoImageName=item.LogoImageName;
                        LogoImageType  = item.LogoImageType;
                        ClientId = item.ClientId;
                        Id = item.ApplicationId;
                        ModifyBy = item.ModifiedBy;
                        Modifydate = item.ModifyDateTime;
                        if (item.LogoImageType.Equals(".svg"))
                        {
                            ImagePath = "data:image/svg+xml;base64,";
                        }
                        else if (item.LogoImageType.Equals(".png"))
                        {
                            ImagePath = "data:image/png;base64,";
                        }
                        else if (item.LogoImageType.Equals(".jpg"))
                        {
                            ImagePath = "data:image/jpg;base64,";
                        }
                        ImagePath += System.Convert.ToBase64String(item.LogoImage);
                    }
                    application_state = LoadingContainerState.Loaded;
                }
            }
            catch (Exception e)
            {
                throw;
            }
        }

        protected override async Task OnAfterRenderAsync(bool firstRender)
        {
            if (firstRender)
            {
                var parameters = new Image_properties()
                {
                    image_picker_id = "#updateapplicationimageupload",
                    image_preview_id = "#update-application-image",
                    image_text_id = "#update-application-text"
                };

                await js.InvokeVoidAsync("loadimagefromdb", parameters);
            }
            base.OnAfterRenderAsync(firstRender);
        }
    }
}
