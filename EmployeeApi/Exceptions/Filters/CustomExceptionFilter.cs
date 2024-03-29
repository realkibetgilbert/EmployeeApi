﻿using EmployeeApi.Helpers;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Net;

namespace EmployeeApi.Exceptions.Filters
{
    [AttributeUsage(AttributeTargets.Class|AttributeTargets.Method)]
    public class CustomExceptionFilter:ExceptionFilterAttribute
    {
        
        public override void OnException(ExceptionContext context)
        {
            context.HttpContext.Response.ContentType= "application/json";

            //var statusCode = HttpStatusCode.InternalServerError;

            //if(context.Exception is StudentNameException)
            //{
            //    statusCode = HttpStatusCode.NotFound;
            //}

            var exceptionString = new ErrorResponseData()
            {
                StatusCode = (int)HttpStatusCode.InternalServerError,
                Message = context.Exception.Message,
                Path = context.Exception.StackTrace
            }.ToString();

            context.Result= new JsonResult(exceptionString);
        }
    }
}
