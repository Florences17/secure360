namespace Opteamix.AuthorizationFramework.BlazorApp.Models.Applications
{
    public class AddApplicationModel
    {
        public string? Name { get; set; }
        public string? Code { get; set; }
        public string? Abbreviation { get; set; }        
        public string? Description { get; set; }
        public byte[]? LogoImage { get; set; }
        public string? LogoImageName { get; set; }
        public string? LogoImageType { get; set; }
        public int ClientId { get; set; }
        public int Id { get; set; }
    }
}
