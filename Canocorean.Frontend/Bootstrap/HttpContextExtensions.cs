using System;
using Microsoft.AspNetCore.Http;

namespace Canocorean.Frontend.Bootstrap
{
    public static class HttpContextExtensions
    {
        public static bool IsAjaxRequest(this HttpContext context)
        {
            if (context?.Request == null)
                throw new ArgumentNullException(nameof(context));

            if (context.Request.Headers != null)
                return context.Request.Headers["X-Requested-With"] == "XMLHttpRequest";
            return false;
        }
    }
}