namespace Opteamix.AuthorizationFramework.BlazorApp.Models.Client
{
    public class Client
     {
        public int ClientId { get; set; }

        public string Name { get; set; }

        public string ShortName { get; set; }

        public string? ContactPerson{get; set;}

        public byte[]? LogoImage { get; set; }

    }
}
