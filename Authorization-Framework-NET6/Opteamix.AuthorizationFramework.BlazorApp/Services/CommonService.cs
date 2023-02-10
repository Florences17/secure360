using Microsoft.AspNetCore.Components;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Applications;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Clients;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Roles;

namespace Opteamix.AuthorizationFramework.BlazorApp.Services
{
    public class CommonService
    {
        public event Action OnChange;

        [Inject]
        public HttpClient httpClient { get; set; }

        public FetchClientModel SelectedClient { get; private set; }

        public FetchRolesModel SelectedRole { get; private set; }

        public void SetSelectedClient(FetchClientModel model)
        {
            SelectedClient = model;
            if (SelectedClient.ClientLogoImageType.Equals(".svg"))
            {
                SelectedClient.ImagePath = "data:image/svg+xml;base64,";
            }
            else if (SelectedClient.ClientLogoImageType.Equals(".png"))
            {
                SelectedClient.ImagePath = "data:image/png;base64,";
            }
            else if (SelectedClient.ClientLogoImageType.Equals(".jpg"))
            {
                SelectedClient.ImagePath = "data:image/jpg;base64,";
            }
            SelectedClient.ImagePath += System.Convert.ToBase64String(model.LogoImage);
           
            NotifyStateChanged();
        }

        public void SetSelectedRole(FetchRolesModel model)
        {
            SelectedRole = model;
            NotifyStateChanged();
        }

        public List<Application> Applications { get; private set; } = new List<Application>();

        public void UpdateApplicationImage()
        {
            foreach (var app in Applications)
            {
                if (app.LogoImageType.Equals(".svg"))
                {
                    app.ImagePath = "data:image/svg+xml;base64,";
                }
                else if (app.LogoImageType.Equals(".png"))
                {
                    app.ImagePath = "data:image/png;base64,";
                }
                else if (app.LogoImageType.Equals(".jpg"))
                {
                    app.ImagePath = "data:image/jpg;base64,";
                }
                app.ImagePath += System.Convert.ToBase64String(app.LogoImage);
            }
            NotifyStateChanged();
        }

        public void SetApplication(Application item)
        {
            Applications.Add(item);
        }

        public Application SelectedApplication { get; private set; }
        

        public void SetSelectedApplication(Application application)
        {
            SelectedApplication = application;
            NotifyStateChanged();
        }

        private void NotifyStateChanged() => OnChange?.Invoke();

        public int SelectedModule { get; private set; }

        public void SetSelectedModule(int id)
        {
            SelectedModule = id;
            NotifyStateChanged();
        }

        public int SelectedPrivilege { get; private set; }

        public void SetSelectedPrivilege(int id)
        {
            SelectedPrivilege = id;
            NotifyStateChanged();
        }
    }
}
