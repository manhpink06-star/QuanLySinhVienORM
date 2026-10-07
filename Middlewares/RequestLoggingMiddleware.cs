using Microsoft.AspNetCore.Http;
using System.Diagnostics;

namespace QuanLySinhVienORM.Middlewares
{
    public class RequestLoggingMiddleware
    {
        private readonly RequestDelegate _next;

        public RequestLoggingMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            
            var startTime = DateTime.Now;

            
            var stopwatch = Stopwatch.StartNew();

            var method = context.Request.Method;
            var path = context.Request.Path.ToString();

            Console.WriteLine(
                $"[{startTime:yyyy-MM-dd HH:mm:ss.fff}] " +
                $"Method: {method} - Path: {path}"
            );

           
            if (path.StartsWith(
                "/SinhViens/Details/",
                StringComparison.OrdinalIgnoreCase))
            {
                var segments = path.Split(
                    '/',
                    StringSplitOptions.RemoveEmptyEntries);

                if (segments.Length >= 3 &&
                    int.TryParse(segments[2], out int id) &&
                    id <= 0)
                {
                    context.Response.StatusCode =
                        StatusCodes.Status400BadRequest;

                    context.Response.ContentType =
                        "text/plain; charset=utf-8";

                    await context.Response.WriteAsync(
                        "Sinh viên id không hợp lệ"
                    );

                    stopwatch.Stop();

                    Console.WriteLine(
                        $"Status Code: {context.Response.StatusCode}"
                    );

                    Console.WriteLine(
                        $"Processing Time: {stopwatch.ElapsedMilliseconds} ms"
                    );

                    return;
                }
            }


            await _next(context);

           

            stopwatch.Stop();

            Console.WriteLine(
                $"Status Code: {context.Response.StatusCode}"
            );

            Console.WriteLine(
                $"Processing Time: {stopwatch.ElapsedMilliseconds} ms"
            );

            Console.WriteLine(
                "------------------------------------------"
            );
        }
    }
}