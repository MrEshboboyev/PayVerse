using PayVerse.Infrastructure.Authentication;
using Microsoft.Extensions.Options;

namespace PayVerse.App.OptionsSetup;

public class JwtOptionsSetup(IConfiguration configuration) : IConfigureOptions<JwtOptions>
{
    private const string SectionName = "Jwt";

    public void Configure(JwtOptions options)
    {
        configuration.GetSection(SectionName).Bind(options);
        
        #region Using this code, if using .env file variables
        
        // Update the secret key from the environment variable
        var envSecretKey = Environment.GetEnvironmentVariable("JWT_SECRET_KEY");
        if (!string.IsNullOrEmpty(envSecretKey))
        {
            options.SecretKey = envSecretKey;
        }
        
        #endregion
    }
}