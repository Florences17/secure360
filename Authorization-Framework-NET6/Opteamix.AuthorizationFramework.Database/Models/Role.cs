using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opteamix.AuthorizationFramework.Data.Model
{
    [Table("Role")]
    public partial class Role
    {
        public Role()
        {
            RoleEntities = new HashSet<RoleEntity>();
        }

        [Key]
        public int Id { get; set; }
        [Required]
        [StringLength(20)]
        public string RoleName { get; set; }
        [StringLength(50)]
        public string? ShortName { get; set; }
        [StringLength(50)]
        public string? RoleDescription { get; set; }
        [Column("Application_Id")]
        public int ApplicationId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }

        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(ApplicationId))]
        [InverseProperty("Roles")]
        public virtual Application Application { get; set; }
        [InverseProperty(nameof(RoleEntity.Role))]
        public virtual ICollection<RoleEntity> RoleEntities { get; set; }
    }
}
