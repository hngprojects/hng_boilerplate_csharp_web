using Hng.Domain.Entities;
using Hng.Infrastructure.Utilities.Results;

namespace Hng.Infrastructure.Services.Interfaces;

public interface IMessageQueueService
{
    public Task<Result<Message>> SendInviteEmailAsync(string inviterName, string inviteeEmail, string organizationName, DateTimeOffset expiryDate, Guid inviteLink);

}
