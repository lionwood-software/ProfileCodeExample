using System.Threading.Tasks;
using Azure.Messaging.ServiceBus;
using MediatR;
using Microsoft.Azure.WebJobs;
using Microsoft.Extensions.Logging;
using Newtonsoft.Json;
using Profile.Adapters.Model;
using Profile.Core.Users.PersonalInfo.UpdateNeoLmsUser;

namespace Profile.Adapters
{
    public class UpdateUserProfile
    {
        private readonly ILogger<UpdateUserProfile> logger;
        private readonly IMediator _mediator;

        public UpdateUserProfile(ILogger<UpdateUserProfile> log, IMediator mediator)
        {
            logger = log;
            _mediator = mediator;
        }

        [FunctionName("UpdateUserProfile")]
        public async Task RunAsync([ServiceBusTrigger("%Topic%", "%Subscription%", Connection = "ServiceBusConnectionString")] ServiceBusReceivedMessage busMessage)
        {
            logger.LogInformation($"{nameof(UpdateUserProfile)}: Message was received {busMessage.Body.ToString()}");

            if (busMessage.Subject == "NeoUserUpsertCompleted")
            {
                var message = JsonConvert.DeserializeObject<ProfileEventPayload>(busMessage.Body.ToString());

                await _mediator.Send(new UpdateNeoIdCommand
                {
                    UserId = message.UserId,
                    NeoId = message.NeoId
                });

                logger.LogInformation(
                    "Message was handled by UpdateNeoUserCommandHandler. UserId = {UserId}, NeoId = {NeoId}", message.UserId, message.NeoId);
                return;
            }

            logger.LogInformation("Messages was not handled because action is not appropriate");
        }
    }
}
