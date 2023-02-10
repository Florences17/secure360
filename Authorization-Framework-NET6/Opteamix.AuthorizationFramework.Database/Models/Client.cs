using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using Microsoft.EntityFrameworkCore;
using Newtonsoft.Json;

namespace Opteamix.AuthorizationFramework.Data.Model
{
    [Table("Client")]
    [Index(nameof(Id), Name = "UK_Client_ShortName", IsUnique = true)]
    public partial class Client
    {
        public Client()
        {
            Applications = new HashSet<Application>();
        }

        [Key]
        public int Id { get; set; }
        [StringLength(100)]
        public string Name { get; set; }
        [StringLength(5)]
        public string ShortName { get; set; }
        public int? ClientStatus { get; set; }
        public int? CreatedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? CreatedDate { get; set; }
        public int? ModifiedBy { get; set; }
        [Column(TypeName = "datetime")]
        public DateTime? ModifiedDate { get; set; }
        public bool? IsDeleted { get; set; }

        [InverseProperty(nameof(Application.Client))]
        public virtual ICollection<Application> Applications { get; set; }

        [StringLength(100)]
        public string? ContactPerson { get; set; }
        public byte[]? ClientLogoImage { get; set; }
        [StringLength(500)]
        public string? ClientLogoName { get; set; }
        [StringLength(30)]
        public string? ClientLogoImageType { get; set; }
        [StringLength(500)]
        public string? AddressLine1 { get; set; }

        [StringLength(500)]
        public string? AddressLine2 { get; set; }
        [StringLength(50)]
        public string? City { get; set; }
        [StringLength(100)]
        public string? WebsiteAddress { get; set; }
        [StringLength(100)]
        public string? EmailAddress { get; set; }
        [StringLength(200)]
        public string? ContactPersonDetails { get; set; }
        [StringLength(100)]
        public string? ContactPersonEmailAddress { get; set; }
        public string? ContactPersonPhoneNumber { get; set; }


    }
}