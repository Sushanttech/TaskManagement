using Microsoft.AspNetCore.Http;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using TaskManager.Api.Data;
using TaskManager.Api.Models;

namespace TaskManager.Api.Middleware
{
    public class RequestResponseLoggingMiddleware
    {
        private readonly RequestDelegate _next;
        public RequestResponseLoggingMiddleware(RequestDelegate next) => _next = next;

        public async Task Invoke(HttpContext context, AppDbContext db)
        {
            context.Request.EnableBuffering();

            string requestBody = "";
            using (var reader = new StreamReader(context.Request.Body, Encoding.UTF8, leaveOpen: true))
            {
                requestBody = await reader.ReadToEndAsync();
                context.Request.Body.Position = 0;
            }

            var originalBodyStream = context.Response.Body;
            await using var responseBody = new MemoryStream();
            context.Response.Body = responseBody;

            await _next(context);

            context.Response.Body.Seek(0, SeekOrigin.Begin);
            string responseBodyText = await new StreamReader(context.Response.Body).ReadToEndAsync();
            context.Response.Body.Seek(0, SeekOrigin.Begin);

            await responseBody.CopyToAsync(originalBodyStream);

            var username = context.User?.Identity?.IsAuthenticated == true ? context.User.Identity!.Name : null;

            var log = new RequestResponseLog
            {
                Method = context.Request.Method,
                Path = context.Request.Path + context.Request.QueryString,
                RequestBody = requestBody?.Length > 4000 ? requestBody.Substring(0, 4000) : requestBody,
                ResponseBody = responseBodyText?.Length > 4000 ? responseBodyText.Substring(0, 4000) : responseBodyText,
                ResponseStatusCode = context.Response.StatusCode,
                Username = username
            };

            db.RequestResponseLogs.Add(log);
            await db.SaveChangesAsync();
        }
    }
}
