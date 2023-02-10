namespace Opteamix.AuthorizationFramework.BlazorApp.Models.Clients
{
    public class FetchClientModel
    {
        public int ClientId { get; set; }

        public string ClientName { get; set; }

        public string ShortName { get; set; }

        public string? ContactPerson { get; set; }

        public byte[]? LogoImage { get; set; }

        public string? ClientLogoImageType { get; set; }

        public string? ImagePath { get; set; }
    }
}
