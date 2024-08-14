using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Infrastructure.Utilities;
using Microsoft.Extensions.Logging;

namespace Hng.Infrastructure.Services;

public class MessageQueueService(ILogger<MessageQueueService> logger, IRepository<Message> repository, IEmailTemplateService templateService) : IMessageQueueService
{
    private readonly ILogger<MessageQueueService> logger = logger;
    private readonly IEmailTemplateService templateService = templateService;

    private async Task<Message> QueueEmailAsync(Message message)
    {
        logger.LogInformation("Now queuing email with id : {emailId}", message.Id);

        await repository.AddAsync(message);

        await repository.SaveChanges();

        return message;
    }

    public async Task<Result<Message>> SendInviteEmailAsync(
        string inviterName,
        string inviteeEmail,
        string organizationName,
        DateTimeOffset expiryDate,
        Guid inviteLink)
    {
        string rawTemplate = await templateService.GetOrganizationInviteTemplate();
        string replacedTemplate = rawTemplate
        .Replace("{{INVITER_NAME}}", inviterName)
        .Replace("{{ORGANIZATION_NAME}}", organizationName)
        .Replace("{{INVITE_LINK}}", inviteLink.ToString())
        .Replace("{{EXPIRY_DATE}}", expiryDate.ToString("dd/M/yyyy hh:mm"));

        Message inviteEmail = Message.CreateEmail(inviteeEmail, $"You've received an invite to join {organizationName}", replacedTemplate);

        Message result = await QueueEmailAsync(inviteEmail);

        return Result<Message>.Success(result);
    }
}
