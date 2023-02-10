namespace Opteamix.AuthorizationFramework.BlazorApp.Models.Clients
{
    public class AddClientModel
    {
        public int? ClientId { get; set; }
        public string Name { get; set; }

        public string ShortName { get; set; }

        public int? ClientStatus { get; set; }

        public bool? IsDeleted { get; set; }

        public byte[]? ClientLogoImage { get; set; }

        public string? ClientLogoName { get; set; }

        public string? ClientLogoImageType { get; set; }

        public string? EmailAddress { get; set; }

        public string? WebsiteAddress { get; set; }

        public string? AddressLine1 { get; set; }

        public string? AddressLine2 { get; set; }

        public string? ContactPerson { get; set; }

        public string? ContactPersonEmailAddress { get; set; }

        public string? ContactPersonPhoneNumber { get; set; }

        public string? City { get; set; }

    }
}
