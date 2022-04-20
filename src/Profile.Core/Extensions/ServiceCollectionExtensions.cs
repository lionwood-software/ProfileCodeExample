using System;
using System.Reflection;
using MediatR;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.DependencyInjection;
using Profile.Core;
using Profile.Core.Matching.Extra;
using Profile.Core.Options;
using Profile.Core.SharedKernel;
using Profile.Core.Users.CreateUser;

namespace Profile.Core.Extensions
{
    public static class ServiceCollectionExtensions
    {
        public static IServiceCollection AddCoreInfrastructure(this IServiceCollection services, Action<SupportedLanguagesOptions> supportLangOptions)
        {
            services.AddMediatR(Assembly.GetExecutingAssembly());
            services.AddScoped<CreateUserHandler>();
            services.AddScoped<IMatchingRequirementsChecker, MatchingRequirementsChecker>();

            services.Configure(supportLangOptions);

            return services;
        }

        public static IServiceCollection AddSqlDatabase(this IServiceCollection services, DatabaseOptions databaseOptions)
        {
            var userContext = services.BuildServiceProvider().GetRequiredService<IProfileUserContext>();
            services.AddDbContext<ProfileDbContext>(options =>
            {
                options.UseSqlServer(databaseOptions.ConnectionString);
                options.AddInterceptors(new AuditInterceptor(userContext));
            });
            return services;
        }
    }
}