using FluentValidation;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using Serilog;

namespace Canocorean.Frontend.Bootstrap.Interceptors
{
    public class APIExceptionFilter : ExceptionFilterAttribute
    {
        private readonly IHostingEnvironment _hostingEnvironment;

        public APIExceptionFilter(IHostingEnvironment hostingEnvironment)
        {
            _hostingEnvironment = hostingEnvironment;
        }

        public override void OnException(ExceptionContext context)
        {
            if (!context.HttpContext.IsAjaxRequest())
            {
                return;
            }

            context.ExceptionHandled = true;
            switch (context.Exception)
            {
                case ValidationException ex:
                    context.Result = new ObjectResult(ex.Errors)
                    {
                        StatusCode = StatusCodes.Status400BadRequest
                    };
                    break;
                default:
                    string message;
                    message = _hostingEnvironment.IsProduction() ? "An unexpected error has occurred" : $"{context.Exception.Message}: {context.Exception.StackTrace}";
                    context.Result = new ObjectResult(message)
                    {
                        StatusCode = StatusCodes.Status500InternalServerError
                    };
                    break;
            }
            switch (context.Exception)
            {
                case ValidationException _:
                    break;
                default:
                    var errorSnapshot = new ErrorSnapshot(context.Exception, context.HttpContext);
                    Log.Error("Unhandled exception {@errorSnapshot}", errorSnapshot);
                    break;

            }
        }
    }
}
