using System;
using JetBrains.Annotations;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.Extensions;

namespace Canocorean.Frontend.Bootstrap.Interceptors
{
    [UsedImplicitly(ImplicitUseTargetFlags.WithMembers)]
    public sealed class ErrorSnapshot
    {
        public ErrorSnapshot(Exception ex, HttpContext context)
        {
            Exception = ex;
            var request = context.Request;
            var userIdentity = context.User?.Identity;
            if (request != null)
            {
                UserHostAddress = context.Connection.RemoteIpAddress.ToString();
                RequestUrl = request.GetDisplayUrl();
            }

            if (userIdentity != null)
            {
                UserName = userIdentity.Name;
            }
        }

        public string UserHostAddress { get; set; }
        public string UserName { get; set; }
        public string RequestUrl { get; set; }
        public Exception Exception { get; }
    }
}