using DevQuestions.Application;
using DevQuestions.Infrastructure.ElasticSearch;

namespace DevQuestions.Web;

public static class DependencyInjection
{
    public static IServiceCollection AddProgramDependencies(this IServiceCollection services)
    {
        return services.AddWebDependencies()
            .AddApplication()
            .AddElastic()
            .AddSqlServerInfrastructure();
    }

    private static IServiceCollection AddWebDependencies(this IServiceCollection services)
    {
        services.AddControllers();
        services.AddOpenApi();

        return services;
    }
}