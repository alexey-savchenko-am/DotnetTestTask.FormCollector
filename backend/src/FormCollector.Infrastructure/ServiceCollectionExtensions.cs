using FormCollector.Domain;
using FormCollector.Infrastructure.Data;
using FormCollector.Infrastructure.Data.Repositories;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Options;

namespace FormCollector.Infrastructure;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddInfrastructure(this IServiceCollection services)
    {
        services.ConfigureOptions<FormCollectorDbOptionsSetup>();

        services.AddDbContext<DbContext, FormCollectorDbContext>((provider, builder) =>
        {
            var options = provider.GetRequiredService<IOptions<DatabaseOptions>>().Value;
            builder.UseNpgsql(options.ConnectionString, actions =>
            {
                if (options.MaxRetryCount is not null && options.MaxRetryCount > 0)
                {
                    actions.EnableRetryOnFailure();
                }
                actions.CommandTimeout(options.CommandTimeout);
            });
            builder.EnableDetailedErrors(options.EnableDetailedErrors);
            builder.EnableSensitiveDataLogging(options.EnableSensitiveDataLogging);
        });

        services.AddScoped<ISubmissionRepository, SubmissionRepository>();

        return services;
    }
}
