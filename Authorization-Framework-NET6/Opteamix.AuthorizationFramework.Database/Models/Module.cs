using Newtonsoft.Json;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opteamix.AuthorizationFramework.Data.Model
{
    [Table("Module")]
    public partial class Module
    {
        public Module()
        {
            Entities = new HashSet<Entity>();
        }

        [Key]
        public int Id { get; set; }
        public int ApplicationId { get; set; }
        [Required]
        [StringLength(20)]
        public string Name { get; set; }
        [StringLength(50)]
        public string? Description { get; set; }
        [StringLength(10)]
        public string? Abbreviation { get; set; }
        [StringLength(50)]
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }

        [ForeignKey(nameof(ApplicationId))]
        [InverseProperty(nameof(Application.Modules))]
        public virtual Application App { get; set; }
        [InverseProperty(nameof(Entity.Module))]
        public virtual ICollection<Entity> Entities { get; set; }
    }
}
