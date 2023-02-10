using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Data.Model
{
    [Table("User")]
    public partial class User
    {

        [Key]
        public int Id { get; set; }

        [Required]
        [StringLength(50)]
        public string FirstName { get; set; }

        [StringLength(50)]
        public string LastName { get; set; }

        [StringLength(100)]
        public string FullName { get; set; }

        [StringLength(100)]
        public string UserName { get; set; }

        [StringLength(255)]
        public string Email { get; set; }

        [StringLength(600)]
        public string Password { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        public int? ManagerId { get; set; }
        
        public int? RoleId { get; set; }

        public bool IsDeleted { get; set; }

        public int ClientId { get; set; }

    }
}
