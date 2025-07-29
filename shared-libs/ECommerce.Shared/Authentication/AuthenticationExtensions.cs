using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.AspNetCore.Builder;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.IdentityModel.Tokens;
using System.Text;

namespace ECommerce.Shared.Authentication;
public static class AuthenticationExtensions
{
    public const string SecurityKey = "kR^86SSZu&10RQ1%^k84hii1poPW^CG*";

    public static void AddJwtAuthentication(this IServiceCollection services, IConfigurationManager configuration)
    {
        var authOptions = new AuthOptions();
        configuration.GetSection(AuthOptions.AuthenticationSectionName).Bind(authOptions);
        services.AddSingleton(authOptions);

        services.AddAuthentication(opt =>
        {
            opt.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
            opt.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
        })
        .AddJwtBearer(options =>
        {
            options.TokenValidationParameters = new TokenValidationParameters
            {
                ValidateIssuer = true,
                ValidIssuer = authOptions.AuthMicroserviceBaseAddress,
                ValidateAudience = false,
                ValidateIssuerSigningKey = true,
                IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(SecurityKey))
            };
        });

        services.AddAuthorization();
    }

    public static void UseJwtAuthentication(this WebApplication app) => app.UseAuthentication().UseAuthorization();
}
