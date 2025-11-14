using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;

namespace FormCollector.Infrastructure;

internal class FormCollectorDbOptionsSetup
    : IConfigureOptions<DatabaseOptions>
{
    private const string DbName = "FormCollectorDb";
    private const string ConfigurationSectionName = "DatabaseOptions";
    private readonly IConfiguration _configuration;

    public FormCollectorDbOptionsSetup(IConfiguration configuration)
    {
        _configuration = configuration;
    }

    public void Configure(DatabaseOptions options)
    {
        var connectionString = _configuration.GetConnectionString(DbName);

        if (connectionString is not null)
        {
            options.ConnectionString = connectionString;
        }

        _configuration.GetSection(ConfigurationSectionName).Bind(options);
    }
}
