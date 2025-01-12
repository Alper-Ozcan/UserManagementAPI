using Microsoft.AspNetCore.Http;
using System.Threading.Tasks;

namespace UserManagement.Middlewares
{
    public class AuthenticationMiddleware
    {
        private readonly RequestDelegate _next;

        public AuthenticationMiddleware(RequestDelegate next)
        {
            _next = next;
        }

        public async Task InvokeAsync(HttpContext context)
        {
            var path = context.Request.Path.Value;
            Console.WriteLine($"AuthenticationMiddleware Path : {path}");
            // Null kontrolü ekleyin
            if (path != null && (path.Contains("/index.html") || path.Contains("/static/") || path.Contains("/auth/login"))) 
            {
                await _next(context);
                return;
            }

            var token = context.Request.Headers["Authorization"].FirstOrDefault()?.Split(" ").Last();
            //var token = context.Request.Headers["Authorization"];

            Console.WriteLine($"Tokenın son parça değer : {token}");

            if (string.IsNullOrEmpty(token))
            {
                context.Response.StatusCode = 401;
                await context.Response.WriteAsync("Unauthorized");
                return;
            }

            await _next(context);
        }


        private bool ValidateToken(string token)
        {
            // Token doğrulama işlemi
            return token == "valid-token";
        }
    }
}
