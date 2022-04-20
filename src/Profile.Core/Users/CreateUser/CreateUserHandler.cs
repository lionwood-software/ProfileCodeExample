using System.Text.Json;
using System.Threading.Tasks;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;

namespace Profile.Core.Users.CreateUser
{
    public class CreateUserHandler
    {
        private const int UniqueConstraintErrorNumber = 2627;
        private const int DuplicatedKeyRowErrorNumber = 2601;

        private readonly ProfileDbContext dbContext;
        private readonly ILogger<CreateUserHandler> logger;

        public CreateUserHandler(ProfileDbContext dbContext, ILogger<CreateUserHandler> logger)
        {
            this.dbContext = dbContext;
            this.logger = logger;
        }

        public async Task Handle(CreateUserRecordCommand command)
        {
            try
            {
                var existedUser = await dbContext.Users.FindAsync(command.UserId);
                if (existedUser != null) { return; }

                dbContext.Users.Add(new User
                {
                    Id = command.UserId,
                    Email = command.Email,
                    FirstName = command.FirstName,
                    LastName = command.LastName,
                    LanguageCode = command.LanguageCode,
                });

                await dbContext.SaveChangesAsync();
            }
            catch (DbUpdateException ex)
                when (ex.InnerException?.InnerException is SqlException sqlEx &&
                    (sqlEx.Number == DuplicatedKeyRowErrorNumber || sqlEx.Number == UniqueConstraintErrorNumber))
            {
                logger.LogWarning("User {user} already exists", JsonSerializer.Serialize(command));
            }
        }
    }
}
