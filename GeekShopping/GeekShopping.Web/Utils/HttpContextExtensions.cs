using Microsoft.AspNetCore.Authentication;

namespace GeekShopping.Web.Utils
{
    public static class HttpContextExtensions
    {
        public static async Task<string> GetContextTokenAsync(this HttpContext httpContext) 
            => await httpContext.GetTokenAsync("access_token") ?? "";
    }
}
