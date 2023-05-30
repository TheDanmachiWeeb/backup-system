using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BackupSystem.Controllers
{
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public bool admin { get; set; } = false;
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                string token = context.HttpContext.Request.Headers["Authorization"].ToString().Split(' ').Last();

                var jwt = JwtBuilder.Create()
                         .WithAlgorithm(new HMACSHA256Algorithm())
                         .WithSecret("backpussy69")
                         .MustVerifySignature()
                         .Decode<IDictionary<string, object>>(token);

                string role = jwt["role"].ToString();

                if (admin && role != "admin")
                {
                    context.Result = new JsonResult(new { message = "Access denied" })
                    {
                        StatusCode = StatusCodes.Status403Forbidden
                    };
                    return;
                }

                context.HttpContext.Items["role"] = role;

            }
            catch
            {
                context.Result = new JsonResult(new { message = "Invalid token" })
                {
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }
        }
    }
}