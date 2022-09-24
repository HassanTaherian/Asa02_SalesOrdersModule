using System;
using System.Net;
using Asa02_SalesOrdersModule.Models;
using Domain.Exceptions;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http;

namespace Asa02_SalesOrdersModule.Extensions
{
    public static class ExceptionMiddlewareExtensions
    {
        public static void ConfigureExceptionHandler(this IApplicationBuilder app, IWebHostEnvironment environment)
        {
            if (environment.IsDevelopment())
            {
                app.UseSwagger();
                app.UseSwaggerUI();
                app.UseExceptionHandler(appError =>
                {
                    appError.Run(async context =>
                    {
                        context.Response.ContentType = "application/json";
                        var contextFeature = context.Features.Get<IExceptionHandlerFeature>();


                        if (contextFeature is not null)
                        {
                            context.Response.StatusCode = contextFeature.Error switch
                            {
                                BadRequestException => StatusCodes.Status400BadRequest,
                                NotFoundException => StatusCodes.Status404NotFound,
                                _ => StatusCodes.Status500InternalServerError
                            };

                            await context.Response.WriteAsync(new Error
                            {
                                StatusCode = context.Response.StatusCode,
                                Message = contextFeature.Error.Message
                            }.ToString());
                        }
                    });
                });
            }
        }
    }
}