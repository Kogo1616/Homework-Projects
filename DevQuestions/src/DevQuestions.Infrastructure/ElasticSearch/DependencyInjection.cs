using DevQuestions.Application.FullTextSearch;
using Microsoft.Extensions.DependencyInjection;

namespace DevQuestions.Infrastructure.ElasticSearch;

public static class DependencyInjection
{
    public static IServiceCollection AddElastic(this IServiceCollection services)
    {
        services.AddScoped<ISearchProvider, ElasticSearchProvider>();

        return services;
    }
}