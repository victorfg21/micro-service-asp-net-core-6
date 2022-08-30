using Microsoft.AspNetCore.Authentication;
using System.Net.Http.Headers;

namespace GeekShopping.Web.Utils
{
    public static class HttpContextExtensions
    {
        public static async Task<string> GetContextTokenAsync(this HttpContext httpContext) 
            => await httpContext.GetTokenAsync("access_token") ?? "";

        public static void SetHeaderRequestToken(this HttpClient httpClient, string token)
            => httpClient.DefaultRequestHeaders.Authorization = new AuthenticationHeaderValue("Bearer", token);
    }
}
