using DevQuestions.Application.Questions;
using DevQuestions.Infrastructure;
using DevQuestions.Infrastructure.SqlServer.Repositories;
using FluentValidation;
using Microsoft.Extensions.DependencyInjection;

namespace DevQuestions.Application;

public static class DependencyInjection
{
    public static IServiceCollection AddSqlServerInfrastructure(this IServiceCollection services)
    {
        services.AddDbContext<QuestionsDbContext>();

        services.AddScoped<IQuestionsRepository, QuestionsRepository>();

        return services;
    }
}