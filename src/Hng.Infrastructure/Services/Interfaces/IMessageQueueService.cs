using Hng.Domain.Entities;
using Hng.Infrastructure.Utilities.Results;

namespace Hng.Infrastructure.Services.Interfaces;

public interface IMessageQueueService
{
    public Task<Result<Message>> SendInviteEmailAsync(
        string inviterName,
        string inviteeEmail,
        string organizationName,
        DateTimeOffset expiryDate,
        string inviteLink);

    public Task<Result<Message>> SendForgotPasswordEmailAsync(
        string firstname,
        string email,
        string companyname,
        string resetlink,
        string year);

    public Task<Result<Message>> SendForgotPasswordEmailMobileAsync(
        string firstname,
        string email,
        string companyname,
        string resetCode,
        string year);
}
