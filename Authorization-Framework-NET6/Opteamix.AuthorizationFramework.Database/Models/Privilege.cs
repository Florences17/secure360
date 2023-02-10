using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opteamix.AuthorizationFramework.Data.Model
{
    [Table("Privilege")]
    public partial class Privilege
    {
        public Privilege()
        {
            EntityPrivileges = new HashSet<EntityPrivilege>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string PrivilegeName { get; set; }
        [StringLength(10)]
        public string? PrivilegeType { get; set; }
        [StringLength(500)]
        public string? Description { get; set; }
        public int? DisplayOrder { get; set; }
        public int AppId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(AppId))]
        [InverseProperty(nameof(Application.Privileges))]
        public virtual Application? App { get; set; }
        [InverseProperty(nameof(EntityPrivilege.Privilege))]
        public virtual ICollection<EntityPrivilege> EntityPrivileges { get; set; }
    }
}
