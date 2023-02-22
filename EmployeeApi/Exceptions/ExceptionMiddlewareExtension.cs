using EmployeeApi.Helpers;
using Microsoft.AspNetCore.Diagnostics;
using Microsoft.AspNetCore.Http.Features;
using System.Net;
using System.Net.WebSockets;

namespace EmployeeApi.Exceptions
{
    public static class ExceptionMiddlewareExtension
    {
        //BUILT IN EXCEPTION HANDLER 
        
        public static void ConfigureBuiltInExceptionHandler(this IApplicationBuilder app)
        {
            app.UseExceptionHandler(appError =>
            {
                appError.Run(async context =>
                {
                    var contextFeature = context.Features.Get<IExceptionHandlerFeature>();

                    var contextRequest = context.Features.Get<IHttpRequestFeature>();

                    context.Response.ContentType = "application/json";  

                    if (contextFeature != null)
                    {
                        var errorString = new ErrorResponseData()
                        {
                            StatusCode = (int)HttpStatusCode.InternalServerError,
                            Message = contextFeature.Error.Message,
                            Path = contextRequest.Path

                        }.ToString();

                        await context.Response.WriteAsync(errorString);

                        

                    }
                });

            });
        }

        public static void ConfigureCustomExceptionHandler(this IApplicationBuilder app)
        {
            app.UseMiddleware<CustomExceptionHandler>();    
        }
    }
}

