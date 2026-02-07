using Microsoft.AspNetCore.Builder;

namespace DevQuestions.Web.Middlewares;

public static class ExceptionMiddlewareExtensions
{
    public static WebApplication UseExceptionMiddleware(this WebApplication app)
    {
        app.UseMiddleware<ExceptionMiddleware>();
        return app;
    }
}
