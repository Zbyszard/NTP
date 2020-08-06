using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Kopyw.Middleware
{
    public class WebSocketsQueryToken
    {
        private readonly RequestDelegate next;
        public WebSocketsQueryToken(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            var request = httpContext.Request;
            var token = request.Query["access_token"];
            if (!string.IsNullOrEmpty(token))
                request.Headers.Add("Authorization", $"Bearer {token}");
            await next(httpContext);
        }
    }
}
