using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opteamix.AuthorizationFramework.Data.Model
{
    [Table("RoleEntity")]
    public partial class RoleEntity
    {
        [Key]
        public int Id { get; set; }
        [Column("Role_Id")]
        public int RoleId { get; set; }
        [Column("Permissionprivilege_Id")]
        public int PermissionprivilegeId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }

        [ForeignKey(nameof(PermissionprivilegeId))]
        [InverseProperty(nameof(EntityPrivilege.RoleEntities))]
        public virtual EntityPrivilege Permissionprivilege { get; set; }

        [ForeignKey(nameof(RoleId))]
        [InverseProperty("RoleEntities")]
        public virtual Role Role { get; set; }

    }
}
