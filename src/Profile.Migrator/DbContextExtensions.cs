using Microsoft.EntityFrameworkCore;
using Profile.Core;

namespace Profile.Migrator
{
    public static class DbContextExtensions
    {
        public static Task EnableIdentityInsert<T>(this ProfileDbContext context) => SetIdentityInsert<T>(context, enable: true);
        public static Task DisableIdentityInsert<T>(this ProfileDbContext context) => SetIdentityInsert<T>(context, enable: false);

        private static Task SetIdentityInsert<T>(ProfileDbContext context, bool enable)
        {
            if (!context.Database.IsRelational())
            {
                return Task.CompletedTask;
            }

            var mapping = context.Model.FindEntityType(typeof(T));
            var value = enable ? "ON" : "OFF";

            return context.Database.ExecuteSqlRawAsync($"SET IDENTITY_INSERT {mapping.GetSchema()}.{mapping.GetTableName()} {value}");
        }

        public static Task ResetIdentityCount<T>(this ProfileDbContext context)
        {
            if (!context.Database.IsRelational())
            {
                return Task.CompletedTask;
            }

            var mapping = context.Model.FindEntityType(typeof(T));
            return context.Database.ExecuteSqlRawAsync($"DBCC CHECKIDENT ('[{mapping.GetTableName()}]', RESEED, 0);");
        }
    }
}