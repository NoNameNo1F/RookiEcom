using RookiEcom.Infrastructure.ConfigurationOptions;
using RookiEcom.Infrastructure.Logging;

namespace RookiEcom.WebAPI.ConfigurationOptions;

public class AppSettings
{
    public JwtOptions Jwt { get; set; }
    public ConnectionStringOptions ConnectionString { get; set; }
    public Dictionary<string, string> Storage { get; set; }
    public string AllowedHosts { get; set; }
    public LoggingOptions Logging { get; set; }
}