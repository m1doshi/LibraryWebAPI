using WebAPI.API.ExceptionHandler;

namespace WebAPI.API.Extensions
{
    public static class ExceptionHandlerMiddlewareExtension
    {
        public static void UseExceptionHandlerMiddleware(this IApplicationBuilder app)
        {
            app.UseMiddleware<ExceptionHandlerMiddleware>();
        }
    }
}
