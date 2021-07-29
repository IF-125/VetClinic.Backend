using AutoMapper;
using Microsoft.AspNetCore.Http;
using Microsoft.IdentityModel.Tokens;
using SendGrid.Helpers.Errors.Model;
using System;
using System.Net;
using System.Threading.Tasks;

namespace VetClinic.WebApi.ExceptionHandling
{
    public class ExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        public ExceptionMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext httpContext)
        {
            try
            {
                await _next(httpContext);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(httpContext, ex);
            }
        }

        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            context.Response.ContentType = "application/json";
            string message = "";
            switch (exception)
            {
                case ArgumentException _:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    message = "One of the arguments is invalid. Message - " + exception.Message;
                    break;
                case SecurityTokenException _:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    message = "Security token is invalid.";
                    break;
                case AutoMapperMappingException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    message = $"Some passed values were not in correct format. {ex.Message}";
                    break;
                case NotFoundException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.NotFound;
                    message = ex.Message;
                    break;
                case BadRequestException ex:
                    context.Response.StatusCode = (int)HttpStatusCode.BadRequest;
                    message = ex.Message;
                    break;
                default:
                    context.Response.StatusCode = (int)HttpStatusCode.InternalServerError;
                    message = exception.Message;
                    break;
            }
            await context.Response.WriteAsync(new ErrorDetails()
            {
                StatusCode = context.Response.StatusCode,
                Message = message
            }.ToString());
        }
    }
}
