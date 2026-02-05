using DevQuestions.Application.Questions;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DevQuestions.Application;

public static class DependencyInjection
{
    public static IServiceCollection  AddSqlServerInfrastructure(this IServiceCollection services)
    {
        services.AddScoped<IQuestionsRepository, IQuestionsRepository>();
        return services;
    }
}