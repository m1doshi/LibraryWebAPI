using Newtonsoft.Json;
using System.Net;
using WebAPI.Application.Exceptions;
using WebAPI.Domain.Exceptions;
using WebAPI.Infrastructure.Exceptions;

namespace WebAPI.API.ExceptionHandler
{
    public class ExceptionHandlerMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionHandlerMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext context)
        {
            try
            {
                await next.Invoke(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionMessage(context, ex).ConfigureAwait(false);

            }
        }

        private static Task HandleExceptionMessage(HttpContext context, Exception exception)
        {
            int statusCode = (int)HttpStatusCode.InternalServerError;
            var result = String.Empty;
            switch (exception)
            {
                case ValidationException validationException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(new { errors = validationException.Message });
                    break;
                case BusinessRuleViolationException businessRuleViolationException:
                    statusCode = (int)HttpStatusCode.BadRequest;
                    result = JsonConvert.SerializeObject(new { errors = businessRuleViolationException.Message });
                    break;
                case EntityNotFoundException entityNotFoundException:
                    statusCode= (int)HttpStatusCode.NotFound;
                    result = JsonConvert.SerializeObject(new { errors = entityNotFoundException.Message });
                    break;
                case DatabaseOperationException databaseOperationException:
                    statusCode = (int)HttpStatusCode.InternalServerError;
                    result = JsonConvert.SerializeObject(new { errors = databaseOperationException.Message });
                    break;
                default:
                    result = JsonConvert.SerializeObject(new { errors = "Inner exception" });
                    break;
            }
            context.Response.StatusCode = statusCode;
            context.Response.ContentType = "application/json";
            return context.Response.WriteAsync(result);
        }
    }
}
