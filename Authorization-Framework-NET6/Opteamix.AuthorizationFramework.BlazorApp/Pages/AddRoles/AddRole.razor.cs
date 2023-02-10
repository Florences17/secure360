using System.Threading.Tasks;
using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Roles;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Privilege;
using Opteamix.AuthorizationFramework.BlazorApp.Services;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Modules;

namespace Opteamix.AuthorizationFramework.BlazorApp.Pages.AddRoles
{
    public partial class AddRole
    {

        public string RoleName { get; set; }

        public string ShortName { get; set; }

        public string RoleDescription { get; set; }

        [Inject]
        IAddRoleService AddRoleService { get; set; }

        [Inject]
        CommonService commonService { get; set; }

        [Inject]
        IJSRuntime js { get; set; }

        [Inject]
        public NavigationManager navigationManager { get; set; }

        [Inject]
        IModuleService ModuleService { get; set; }

        public bool AppExpand { get; set; } = false;

        public bool ModuleExpand { get; set; } = false;

        public bool SubModuleExpand { get; set; } = false;

        public List<SubModule> DisplaySubModules { get; set; } = new List<SubModule>();

        public async void SaveButtonClick()
        {
            if (string.IsNullOrEmpty(RoleName))
            {
                await js.InvokeVoidAsync("alert", "Please enter Role name");
                return;
            }
            else
            {
                AddRoleModel addRole = new AddRoleModel
                {
                    RoleName = RoleName,
                    ShortName = ShortName,
                    RoleDescription = RoleDescription,
                    ApplicationId = commonService.SelectedApplication.ApplicationId
                };
                try
                {
                    await AddRoleService.AddRole(addRole);
                    Roleforid = await AddRoleService.GetRoleId(addRole);
                    int roleid = 0;
                    if (Roleforid.Any())
                    {
                        foreach (FetchRolesModel Roles in Roleforid)
                        {
                            roleid = Roles.Id;
                        }

                    }
                    var SubModules = await ModuleService.GetSubModules();
                    foreach (var item in SubModules)
                    {
                        foreach (int[] EntPrev in SelectedRole)
                        {
                            if (EntPrev[1] == item.ModuleId)
                            {

                                EntPrev[3] = item.SubModuleId;
                                break;
                            }
                        }
                    }
                    foreach (int[] EntPrev in SelectedRole)
                    {
                        int moduleId = EntPrev[1];
                        int privilegeId = EntPrev[2];
                        int PermissionId = EntPrev[3];
                        ModulePrivilege addModulePrivilege = new ModulePrivilege
                        {
                            ModuleId = moduleId,
                            PrivilegeId = privilegeId,
                            RoleId = roleid,
                            PermissionId= PermissionId

                        };
                        await AddRoleService.AddModulePrivilege(addModulePrivilege);
                        //ModulePrev = await AddRoleService.GetModPrevId(addModulePrivilege);
                        //await js.InvokeVoidAsync("alert"+ "addRoleModule=" + ModulePrev.Count());
                        //foreach (var element in ModulePrev)
                        //{
                        //    int ModPrevId = element.Id;

                        //    RoleModule addRoleModule = new RoleModule
                        //    {
                        //        ModulePrevilegeId = ModPrevId,
                        //        RoleId = roleid
                        //    };
                        //    await js.InvokeVoidAsync("alert", "ModulePrevilegeId and =" + addRoleModule.ModulePrevilegeId + " " + addRoleModule.RoleId);
                        //    await AddRoleService.AddRoleModule(addRoleModule);
                        //}


                    }

                    navigationManager.NavigateTo(Constants.Roles);
                }
                catch (Exception ex)
                {

                }
            }
        }

        public async void MenuButtonClick(string url)
        {
            navigationManager.NavigateTo(url);
        }

        public async void CancelButtonClick(string url)
        {
            navigationManager.NavigateTo(url);
        }
        public void ToggleAppExpand()
        {
            if (AppExpand == true)
            {
                AppExpand = false;
            }
            else
            {
                AppExpand = true;
            }
        }
        public void ToggleModuleExpand()
        {
            if (ModuleExpand == true)
            {
                ModuleExpand = false;
            }
            else
            {
                ModuleExpand = true;
            }
        }

        public IEnumerable<FetchPrivilegeModel> Privileges { get; set; }
        public IEnumerable<Models.Applications.Application> Applications { get; set; }
        public List<FetchPrivilegeModel> DisplayPrivileges { get; set; } = new List<FetchPrivilegeModel>();

        public IEnumerable<FetchRolesModel> Roleforid { get; set; }

        public IEnumerable<ModulePrivilege> ModulePrev { get; set; }
        public List<Models.Applications.Application> Displayapps { get; set; } = new List<Models.Applications.Application>();

        [Inject]
        IPrivilegeService PrivilegeService { get; set; }
        [Inject]
        IApplicationService ApplicationService { get; set; }
        public enum LoadingContainerState { Loading, Loaded, Error }

        LoadingContainerState privilege_state;
        protected override async Task OnInitializedAsync()
        {
            try
            {
                privilege_state = LoadingContainerState.Loading;
                Privileges = await PrivilegeService.GetPrivileges(commonService.SelectedApplication.ApplicationId);
                if (Privileges.Any())
                {
                    foreach (FetchPrivilegeModel privilege in Privileges)
                    {
                        DisplayPrivileges.Add(privilege);
                    }
                    privilege_state = LoadingContainerState.Loaded;
                }
                else 
                {
                    privilege_state = LoadingContainerState.Loaded;
                }


                Applications = await ApplicationService.GetApplications(commonService.SelectedClient.ClientId);
                if (Applications.Any())
                {
                    foreach (Models.Applications.Application app in Applications.Where(a => a.ApplicationId == commonService.SelectedApplication.ApplicationId))
                    {
                        if(commonService.SelectedApplication.ApplicationId == app.ApplicationId) 
                        {
                            Displayapps.Add(app);
                            Modules = await ModuleService.GetModules(app.ApplicationId);
                            if (Modules.Any())
                            {
                                foreach (var m in Modules)
                                {
                                    DisplayModules.Add(m);
                                }
                            }
                            var SubModules = await ModuleService.GetSubModules();

                            if (SubModules.Any())
                            {
                                foreach (var submodule in SubModules)
                                {
                                    DisplaySubModules.Add(submodule);
                                }
                            }
                        }
                    }
                    //privilege_state = LoadingContainerState.Loaded;
                }
                else
                {
                    // privilege_state = LoadingContainerState.Loaded;
                }
            }
            catch (Exception)
            {
                throw;
            }
        }
        public List<Module> DisplayModules { get; set; } = new List<Module>();
        public IEnumerable<Module> Modules { get; set; }

        public async void loadModules(int id)
        {

            Modules = await ModuleService.GetModules(id);
            if (Modules.Any())
            {
                foreach (var m in Modules)
                {
                    DisplayModules.Add(m);
                }
            }
        }

        public List<int[]> SelectedRole { get; set; } = new List<int[]>();

        void RoleSelected(int applicationid, int? moduleid, int privilegeid, object checkedValue)
        {
            int[] listOfId = new int[4];
            if (moduleid == null)
            {
                listOfId[0] = applicationid;
                listOfId[1] = 0;
                listOfId[2] = privilegeid;
            }
            else
            {
                listOfId[0] = applicationid;
                listOfId[1] = (int)moduleid;
                listOfId[2] = privilegeid;
            }
            if ((bool)checkedValue)
            {
                SelectedRole.Add(listOfId);
            }
            else
            {
                int index = -1; int count = -1;
                foreach (int[] element in SelectedRole)
                {
                    count++;
                    if (element[0] == listOfId[0] && element[1] == listOfId[1] && element[2] == listOfId[2])
                    { index = count; break; }
                }
                if (index > -1) SelectedRole.RemoveAt(index);

            }
        }
    }
}
