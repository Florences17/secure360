namespace Opteamix.AuthorizationFramework.BlazorApp.Models
{
    public class ServiceResponse<T>
    {
        public T Data {get; set;}
        public MetaData MetaData {get; set;}
    }
}