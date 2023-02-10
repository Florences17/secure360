using Microsoft.AspNetCore.Components;
using Microsoft.JSInterop;
using Opteamix.AuthorizationFramework.BlazorApp.Models;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Modules;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Privilege;
using Opteamix.AuthorizationFramework.BlazorApp.Models.Roles;
using Opteamix.AuthorizationFramework.BlazorApp.Services;

namespace Opteamix.AuthorizationFramework.BlazorApp.Pages.Roles
{
    public partial class RoleDetails
    {
        public string RoleName { get; set; }

        public string ShortName { get; set; }

        public string RoleDescription { get; set; }

        public int RoleId { get; set; }

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

        public IEnumerable<FetchRolesModel> Roleforid { get; set; }

        public List<int[]> SelectedRole { get; set; } = new List<int[]>();

        public List<EntityPrivilege> entityPrivilegeslst =
            new List<EntityPrivilege>();
        public List<SubModule> DisplaySubModules { get; set; } = new List<SubModule>();

        public IEnumerable<ModulePrivilege> ModulePrev { get; set; }

        public async Task HandleExpandStatus(bool status)
        {
            ModuleExpand = status;
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

        public async void SaveButtonClick()
        {
            if (string.IsNullOrEmpty(RoleName))
            {
                await js.InvokeVoidAsync("alert", "null");
            }
            else
            {
                AddRoleModel addRole = new AddRoleModel
                {
                    RoleName = RoleName,
                    Id = RoleId,
                    ShortName = ShortName,
                    RoleDescription = RoleDescription,

                };
                try
                {

                    await AddRoleService.UpdateRole(addRole);
                    // Below methods deelted and replicates the Create flow of roles.
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
                            RoleId = RoleId,
                            PermissionId = PermissionId,

                        };
                        await AddRoleService.AddModulePrivilege(addModulePrivilege);
                    }

                    navigationManager.NavigateTo(Constants.Roles);
                }
                catch (Exception ex)
                {
                    await js.InvokeVoidAsync("alert", ex.ToString());
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

        public IEnumerable<FetchPrivilegeModel> Privileges { get; set; }
        public IEnumerable<Models.Applications.Application> Applications { get; set; }
        public List<FetchPrivilegeModel> DisplayPrivileges { get; set; } = new List<FetchPrivilegeModel>();

        public List<Models.Applications.Application> Displayapps { get; set; } = new List<Models.Applications.Application>();

        [Inject]
        IPrivilegeService PrivilegeService { get; set; }
        [Inject]
        IApplicationService ApplicationService { get; set; }
        public enum LoadingContainerState { Loading, Loaded, Error }

        LoadingContainerState privilege_state;

        public IEnumerable<RoleEntity> RoleEntities { get; set; }

        public IEnumerable<EntityPrivilege> EntityPrivileges { get; set; }

        public List<int> SelectedPermissions { get; set; }

        public List<int> SelectedPrivileges { get; set; }

        protected override async Task OnInitializedAsync()
        {
            try
            {
                SelectedPermissions = new List<int>();
                SelectedPrivileges = new List<int>();

                privilege_state = LoadingContainerState.Loading;

                // fetch selected role detail
                var selectedRole = commonService.SelectedRole;
                RoleId = selectedRole.Id;
                RoleName = selectedRole.RoleName;
                ShortName = selectedRole.ShortName;
                RoleDescription = selectedRole.RoleDescription;

                Privileges = await PrivilegeService.GetPrivileges(commonService.SelectedApplication.ApplicationId);
                if (Privileges.Any())
                {
                    Privileges = Privileges.Take(12);
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

                ///// - Fetching All applications for the client
                Applications = await ApplicationService.GetApplications(commonService.SelectedClient.ClientId);
                if (Applications.Any())
                {
                    foreach (Models.Applications.Application app in Applications.Where(a => a.ApplicationId == commonService.SelectedApplication.ApplicationId))
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
                    //privilege_state = LoadingContainerState.Loaded;
                }
                else
                {
                    // privilege_state = LoadingContainerState.Loaded;
                }
                ///// - Fetching saved modules as per the client application logged in
                RoleEntities = await AddRoleService.GetRoleEntityByRoleId(selectedRole.Id);

                foreach (var roleEntity in RoleEntities)
                {
                    //EntityPrivileges = await AddRoleService.GetEntityPrivilegeById(roleEntity.PermissionprivilegeId);
                    EntityPrivileges = await AddRoleService.GetEntityPrivilegeById(roleEntity.PermissionprivilegeId);


                    if (EntityPrivileges.Count() > 0)
                    {
                        entityPrivilegeslst.Add(new EntityPrivilege() { ModuleId = EntityPrivileges.First().ModuleId, PrivilegeId = EntityPrivileges.First().PrivilegeId });

                        SelectedPermissions.Add(EntityPrivileges.First().ModuleId);
                        SelectedPrivileges.Add(EntityPrivileges.First().PrivilegeId);
                    }
                }
                foreach (var app in Displayapps)
                {


                    foreach (var dm in DisplayModules)
                    {
                        if (dm.ApplicationName == app.ApplicationName)
                        {
                            foreach (var dp in DisplayPrivileges)
                            {
                                if (SelectedPermissions.Contains(dm.ModuleId) && SelectedPrivileges.Contains(dp.PrivilegeId))
                                {
                                    RoleSelected(app.ApplicationId, dm.ModuleId, dp.PrivilegeId, true);
                                }
                            }
                        }
                    }
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
                    {
                        index = count; break;
                    }
                }
                if (index > -1) SelectedRole.RemoveAt(index);

            }
        }
    }
}