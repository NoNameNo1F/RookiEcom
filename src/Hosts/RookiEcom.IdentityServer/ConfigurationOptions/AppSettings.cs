﻿namespace RookiEcom.IdentityServer.ConfigurationOptions;

public class AppSettings
{
    public object AllowedHosts { get; set; }
    public ConnectionStringOptions ConnectionStrings { get; set; }
    public JwtOptions Jwt { get; set; }
    public StorageOptions Storage { get; set; }
}