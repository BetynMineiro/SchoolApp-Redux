using System.Net;
using System.Text.Json;
using SchoolApp.Application.ApplicationServices.NotificationService;
using SchoolApp.CrossCutting.ResultObjects;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace SchoolApp.Api.Middleware;

public class NotificationServiceMiddleware : IAsyncResultFilter
{
    private readonly NotificationServiceContext _notificationServiceContext;

    public NotificationServiceMiddleware(NotificationServiceContext notificationServiceContext)
    {
        _notificationServiceContext = notificationServiceContext;
    }
    public async Task OnResultExecutionAsync(ResultExecutingContext context, ResultExecutionDelegate next)
    {
        var response = new Result<object>();
        context.HttpContext.Response.ContentType = "application/json";
        var options = new JsonSerializerOptions
        {
            PropertyNamingPolicy = JsonNamingPolicy.CamelCase
        };
        if (_notificationServiceContext.HasNotifications)
        {
            context.HttpContext.Response.StatusCode = (int)HttpStatusCode.BadRequest;
            response.Success = false;
            response.Messages = _notificationServiceContext.Notifications.Select(x => x.Message).ToList();
            
            await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(response,options));

            return;
        }

        var resultContext = context.Result as ObjectResult;

        if (resultContext?.Value != null)
        {
            response.Success = true;
            response.Data = resultContext.Value;
            await context.HttpContext.Response.WriteAsync(JsonSerializer.Serialize(response,options));

            return;
        }

        await next();
    }
}