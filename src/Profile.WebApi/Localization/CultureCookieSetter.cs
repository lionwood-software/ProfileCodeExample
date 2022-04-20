using System;
using System.Globalization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace Profile.WebApi.Localization;

public static class CultureCookieSetter
{
    public static void SetCurrentUiCulture(HttpContext httpContext)
    {
        var isProduction = httpContext.RequestServices.GetRequiredService<IHostEnvironment>().IsProduction();

        httpContext.Response.Cookies.Append(
            CookieRequestCultureProvider.DefaultCookieName,
            CookieRequestCultureProvider.MakeCookieValue(new RequestCulture(CultureInfo.CurrentUICulture)),
            new CookieOptions
            {
                Expires = DateTime.MaxValue,
                Domain = isProduction ? ".site.com" : "localhost",
                Path = "/",
                HttpOnly = true,
                Secure = true
            }
        );
    }
}