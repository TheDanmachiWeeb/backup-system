using JWT.Algorithms;
using JWT.Builder;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace BackupSystem.Controllers
{
    public class AuthorizeAttribute : Attribute, IAuthorizationFilter
    {
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            try
            {
                string token = context.HttpContext.Request.Headers["Authorization"].ToString().Split(' ').Last();

                var json = JwtBuilder.Create()
                         .WithAlgorithm(new HMACSHA256Algorithm())
                         .WithSecret("backpussy69")
                         .MustVerifySignature()
                         .Decode(token);
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