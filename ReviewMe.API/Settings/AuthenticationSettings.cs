
namespace ReviewMe.API.Settings;

public class AuthenticationSettings
{
    public string? SecretKey { get; set; }

    public int JwtExpirationPeriodInSeconds { get; set; }

    public int JwtClockSkewInSeconds { get; set; }
}