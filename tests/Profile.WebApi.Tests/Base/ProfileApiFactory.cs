using System;
using System.IO;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.Testing;
using Microsoft.AspNetCore.TestHost;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.IdentityModel.Tokens;
using Moq;
using Profile.Core;
using Profile.Core.EventBus;
using Profile.Core.FileManager;
using Profile.Core.SharedKernel;
using Profile.Core.Users;
using Profile.Migrator;
using Serilog;
using Xunit.Abstractions;

namespace Profile.WebApi.Tests.Base
{
    public class ProfileApiFactory<TStartup> : WebApplicationFactory<TStartup>
        where TStartup : class
    {
        public static User ExecutionUser = new()
        {
            Id = Guid.Parse("7f004b22-dd62-4af6-b666-81cd5b305d82"),
            Email = "testuser@gmail.com",
            FirstName = "Extra",
            LastName = "User",
            LanguageCode = "en",
            IsOnboarded = true,
            PhoneNumber = "9379992",
            AvatarUrl = null,
            CreatedBy = "testuser@gmail.com",
            ModifiedBy = "testuser@gmail.com",
            CreatedDate = DateTime.UtcNow,
            ModifiedDate = DateTime.UtcNow
        };

        public Mock<IEventBusPort> ServiceBusMock { get; set; }
        public Mock<IAvatarStorage> AvatarStorageMock { get; set; }

        public ITestOutputHelper Output { get; set; }

        public void ResetMocks()
        {
            ServiceBusMock.Reset();
            AvatarStorageMock.Reset();
        }

        public void CleanDatabase()
        {
            var options = new DbContextOptionsBuilder<ProfileDbContext>().UseInMemoryDatabase("Profile-db").Options;
            using var context = new ProfileDbContext(options, null);

            var user = context.Users.AsNoTracking().SingleOrDefault(x => x.Id == ExecutionUser.Id);
            if (user == null) { context.Users.Add(ExecutionUser); } else { context.Users.Update(ExecutionUser); }

            context.UserExtraCities.RemoveRange(context.UserExtraCities.Where(x => x.UserId == ExecutionUser.Id));

            context.SaveChanges();
        }

        protected override void ConfigureWebHost(IWebHostBuilder builder)
        {
            var path = Assembly.GetExecutingAssembly().Location;

            builder.ConfigureServices((context, services) =>
           {
               MockDb(services).Wait();
               MockServiceBus(services);
               MockAzureStorage(services);
               AddTestJwtSchemeAsDefault(context, services);
           });

            builder.UseContentRoot(Path.GetDirectoryName(path))
                .UseTestServer()
                .ConfigureAppConfiguration(configurationBuilder => configurationBuilder.AddJsonFile("appsettings.test.json", optional: false))
                .UseEnvironment("Development");
        }

        private void MockAzureStorage(IServiceCollection services)
        {
            var avatarStorageDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IAvatarStorage));
            services.Remove(avatarStorageDescriptor);

            AvatarStorageMock = new Mock<IAvatarStorage>();
            services.AddScoped(_ => AvatarStorageMock.Object);
        }

        private static void AddTestJwtSchemeAsDefault(WebHostBuilderContext context, IServiceCollection services)
        {
            var options = context.Configuration.GetSection("Auth").Get<AuthOptions>();
            services.AddAuthentication("TestBearer").AddJwtBearer("TestBearer", o =>
            {
                o.Audience = options.Audience;
                o.Authority = null;
                o.TokenValidationParameters.ValidateAudience = true;
                o.TokenValidationParameters.ValidateIssuer = true;
                o.TokenValidationParameters.ValidIssuer = options.Authority;
                o.TokenValidationParameters.IssuerSigningKey = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(TokenGeneration.Key));
            });
        }

        public string BuildClientToServerToken(string email = null, Guid? userId = null)
        {
            var configuration = Services.GetRequiredService<IConfiguration>();
            var options = configuration.GetSection("Auth").Get<AuthOptions>();
            return TokenGeneration.BuildIndividualToken(options, email ?? ExecutionUser.Email, userId ?? ExecutionUser.Id);
        }

        public string BuildServerToServerToken()
        {
            var configuration = Services.GetRequiredService<IConfiguration>();
            var options = configuration.GetSection("Auth").Get<AuthOptions>();
            return TokenGeneration.BuildServiceToServiceToken(options);
        }

        private async Task MockDb(IServiceCollection services)
        {
            var dbContextDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(ProfileDbContext));
            services.Remove(dbContextDescriptor);

            var dbContextOptions = services.SingleOrDefault(d => d.ServiceType == typeof(DbContextOptions<ProfileDbContext>));
            services.Remove(dbContextOptions);

            services.AddDbContext<ProfileDbContext>(opt =>
            {
                opt.UseInMemoryDatabase(databaseName: "Profile-db");
                opt.AddInterceptors(new AuditInterceptor(new MigrationUserContext()));
            });

            await DataSeeder.Seed(services);
        }

        private void MockServiceBus(IServiceCollection services)
        {
            var eventBusDescriptor = services.SingleOrDefault(d => d.ServiceType == typeof(IEventBusPort));
            services.Remove(eventBusDescriptor);

            ServiceBusMock = new Mock<IEventBusPort>();
            services.AddScoped(_ => ServiceBusMock.Object);
        }

        protected override IHostBuilder CreateHostBuilder() =>
            new HostBuilder()
                .UseSerilog((_, configuration) => configuration.WriteTo.TestOutput(Output))
                .ConfigureWebHostDefaults(webBuilder => { webBuilder.UseStartup<TStartup>(); });
    }
}