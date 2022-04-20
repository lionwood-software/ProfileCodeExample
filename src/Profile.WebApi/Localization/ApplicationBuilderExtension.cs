using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Localization;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Profile.Core;
using Profile.Core.Options;
using Profile.Core.SharedKernel;

namespace Profile.WebApi.Localization;

public static class ApplicationBuilderExtension
{
    public static IApplicationBuilder UseProfileRequestLocalization(this IApplicationBuilder applicationBuilder)
    {
        var configuration = applicationBuilder.ApplicationServices.GetRequiredService<IConfiguration>();
        var options = new SupportedLanguagesOptions
        {
            SupportedLanguages = configuration.GetSection(SupportedLanguagesOptions.SectionName).Get<List<string>>()
        };

        return applicationBuilder.UseRequestLocalization(o =>
        {
            o.DefaultRequestCulture = new RequestCulture("en");
            o.ApplyCurrentCultureToResponseHeaders = true;
            o.SupportedUICultures = options.SupportedLanguages.Select(l => new CultureInfo(l)).ToList();

            o.RequestCultureProviders.Clear();
            o.RequestCultureProviders.Add(new CookieRequestCultureProvider());
            o.RequestCultureProviders.Add(new CustomRequestCultureProvider(async context =>
            {
                var dbContext = context.RequestServices.GetRequiredService<ProfileDbContext>();
                var profileUserContext = context.RequestServices.GetRequiredService<IProfileUserContext>();

                context.Response.OnStarting(UpdateCultureCookie, (context, profileUserContext));

                if (context.User.Identity?.IsAuthenticated is true && profileUserContext.HasUserId())
                {
                    var user = await dbContext.Users.FindAsync(profileUserContext.UserId);
                    if (user != null)
                    {
                        profileUserContext.SetCulture(user.LanguageCode);
                        return new ProviderCultureResult(user.LanguageCode);
                    }
                }

                profileUserContext.SetCulture("en");
                return new ProviderCultureResult("en");
            }));
        });
    }

    private static Task UpdateCultureCookie(object context)
    {
        var (httpContext, userContext) = ((HttpContext, IProfileUserContext))context;
        if (!userContext.HasUserId())
        {
            return Task.CompletedTask;
        }

        CultureCookieSetter.SetCurrentUiCulture(httpContext);

        return Task.FromResult(0);
    }
}