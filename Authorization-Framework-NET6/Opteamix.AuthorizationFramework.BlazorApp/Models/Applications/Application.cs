namespace Opteamix.AuthorizationFramework.BlazorApp.Models.Applications
{
    public class Application
    {
        public int ApplicationId { get; set; }

        public string? ApplicationName { get; set; }

        public string Abbreviation { get; set; }

        public string? Description { get; set; }

        public byte[]? LogoImage { get; set; }
        public string? Code { get; set; }

        public string? LogoImageName { get; set; }

        public string? LogoImageType { get; set; }

        public string? ImagePath { get; set; }
        public int ClientId { get; set; }
        public int? ModifiedBy { get; set; }
        public DateTime ModifyDateTime { get; set; }
    }
}
