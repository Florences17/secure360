using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Opteamix.AuthorizationFramework.Data.Model
{
    public partial class UserEntity
    {
        [Key]
        public int Id { get; set; }

        [Column("User_Id")]
        public int UserId { get; set; }

        [Column("Manager_Id")]
        public int ManagerId { get; set; }

        [Column("RoleEntity_Id")]
        public int RoleEntityId { get; set; }

        public int? CreatedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }

        public int? ModifiedBy { get; set; }

        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }

    }
}
