using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opteamix.AuthorizationFramework.Data.Model
{
    [Table("Application")]
    public partial class Application
    {
        public Application()
        {
            Modules = new HashSet<Module>();
            Privileges = new HashSet<Privilege>();
            Roles = new HashSet<Role>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(20)]
        public string Name { get; set; }
        [Required]
        [StringLength(10)]
        public string Abbreviation { get; set; }
        [StringLength(50)]
        public string? Code { get; set; }
        [StringLength(50)]
        public string? Description { get; set; }
        public bool? Active { get; set; }
        public int ClientId { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        public bool IsDeleted { get; set; }

        [ForeignKey(nameof(ClientId))]
        [InverseProperty("Applications")]
        public virtual Client Client { get; set; }
        [InverseProperty(nameof(Module.App))]
        public virtual ICollection<Module> Modules { get; set; }
        [InverseProperty(nameof(Privilege.App))]
        public virtual ICollection<Privilege> Privileges { get; set; }
        [InverseProperty(nameof(Role.Application))]
        public virtual ICollection<Role> Roles { get; set; }
        public byte[]? LogoImage { get; set; }
        public string? LogoImageName { get; set; }
        public string? LogoImageType { get; set; }


    }
}
