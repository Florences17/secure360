using Newtonsoft.Json;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opteamix.AuthorizationFramework.Data.Model
{
    [Table("Entity")]
    public partial class Entity
    {
        public Entity()
        {
            EntityPrivileges = new HashSet<EntityPrivilege>();
            InverseParentPermission = new HashSet<Entity>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(50)]
        public string DisplayName { get; set; }
        public int? ModuleId { get; set; }
        public int? ParentPermissionId { get; set; }
        [StringLength(10)]
        public string Abbreviation { get; set; }
        public int? PermissionType { get; set; }
        public int? DisplayOrder { get; set; }

        [ForeignKey(nameof(ParentPermissionId))]
        [InverseProperty(nameof(Entity.InverseParentPermission))]
        public virtual Entity ParentPermission { get; set; }
        [InverseProperty(nameof(Entity.ParentPermission))]
        public virtual ICollection<Entity> InverseParentPermission { get; set; }
        [ForeignKey(nameof(ModuleId))]
        [InverseProperty("Entities")]
        public virtual Module Module { get; set; }
        [InverseProperty(nameof(EntityPrivilege.Permission))]
        public virtual ICollection<EntityPrivilege> EntityPrivileges { get; set; }
    }
}
