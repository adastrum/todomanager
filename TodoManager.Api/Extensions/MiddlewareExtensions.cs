using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System;
using TodoManager.Api.Exceptions;

namespace TodoManager.Api.Extensions
{
    public static class MiddlewareExtensions
    {
        public static IApplicationBuilder UseApplicationExceptionHandling(this IApplicationBuilder app) =>
            app.Use(async (context, next) =>
            {
                try
                {
                    await next();
                }
                catch (ApplicationException ex)
                {
                    var statusCode = ex is EntityNotFoundException
                        ? StatusCodes.Status404NotFound
                        : StatusCodes.Status400BadRequest;

                    context.Response.StatusCode = statusCode;

                    await context.Response.WriteAsJsonAsync(new ProblemDetails
                    {
                        Type = "https://tools.ietf.org/html/rfc7231#section-6.5.1",
                        Title = "Error occurred while processing request",
                        Status = statusCode,
                        Detail = ex.Message
                    });
                }
            });
    }
}
