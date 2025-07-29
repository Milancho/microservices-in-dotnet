namespace ECommerce.Shared.Authentication;
public class AuthOptions
{
    public const string AuthenticationSectionName = "Authentication";

    public string AuthMicroserviceBaseAddress { get; set; } = string.Empty;
}
