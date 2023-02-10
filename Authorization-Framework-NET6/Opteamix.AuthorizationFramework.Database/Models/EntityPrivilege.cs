using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opteamix.AuthorizationFramework.Data.Model
{
    [Table("EntityPrivilege")]
    public partial class EntityPrivilege
    {
        public EntityPrivilege()
        {
            RoleEntities = new HashSet<RoleEntity>();
        }

        [Key]
        public int Id { get; set; }
        [Column("Permission_Id")]
        public int? PermissionId { get; set; }
        [Column("Privilege_Id")]
        public int? PrivilegeId { get; set; }

        public int? ModuleId { get; set; }

        [ForeignKey(nameof(PermissionId))]
        [InverseProperty(nameof(Entity.EntityPrivileges))]
        public virtual Entity Permission { get; set; }

        [ForeignKey(nameof(PrivilegeId))]
        [InverseProperty("EntityPrivileges")]
        public virtual Privilege Privilege { get; set; }

        [InverseProperty(nameof(RoleEntity.Permissionprivilege))]
        public virtual ICollection<RoleEntity> RoleEntities { get; set; }
    }
}
