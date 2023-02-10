using System;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Opteamix.AuthorizationFramework.Data.Model
{
    [Table("Audit")]
    public partial class Audit
    {
        [Key]
        public int Id { get; set; }
        [StringLength(50)]
        public string Action { get; set; }
        [StringLength(50)]
        public string Entity { get; set; }
        [StringLength(1000)]
        public string Data { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
    }
}
