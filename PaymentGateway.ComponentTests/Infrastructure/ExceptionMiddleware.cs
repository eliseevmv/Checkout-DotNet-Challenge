﻿using Microsoft.AspNetCore.Http;
using System;
using System.Net;
using System.Net.Mime;
using System.Threading.Tasks;

namespace PaymentGateway.ComponentTests.Infrastructure
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate next;

        public ExceptionMiddleware(RequestDelegate next)
        {
            this.next = next;
        }

        public async Task Invoke(HttpContext httpContext)
        {
            try
            {
                await this.next(httpContext);
            }
            catch (Exception ex)
            {
                httpContext.Response.ContentType = MediaTypeNames.Text.Plain;
                httpContext.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                await httpContext.Response.WriteAsync("Internal server error");
            }
        }
    }
}
