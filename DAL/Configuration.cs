using Microsoft.Extensions.Configuration;

namespace DataAccess;

public static class Configuration
{
    static Configuration() => Init();

    public static string? AdminPassword { get; set; }

    public static string? SqlConnectionString { get; set; }

    public static bool AdminEnabled { get; set; }

    private static void Init()
    {
        var config = new ConfigurationBuilder()
            .SetBasePath(Directory.GetCurrentDirectory())
            .AddJsonFile("appsettings.json", optional: true)
            .AddEnvironmentVariables()
            .Build();

        AdminPassword = config["admin:password"];
        SqlConnectionString = config["ConnectionString:DefaultConnection"];
    }
}
