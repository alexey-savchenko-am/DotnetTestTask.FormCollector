using FormCollector.Application.Abstract;
using FormCollector.Application.Submissions;
using Microsoft.Extensions.DependencyInjection;

namespace FormCollector.Application;

public static class ServiceCollectionExtensions
{
    public static IServiceCollection AddApplication(this IServiceCollection services)
    {
        services.AddScoped<ISubmissionReader, SubmissionService>();
        services.AddScoped<ISubmissionWriter, SubmissionService>();
        return services;
    }
}
