using System.Net.Mail;
using System.Reflection;
using System.Text;
using Hng.Domain.Entities;
using Hng.Infrastructure.EmailTemplates;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Infrastructure.Utilities;
using Hng.Infrastructure.Utilities.Errors.Messages;
using Microsoft.Extensions.Logging;

namespace Hng.Infrastructure.Services;

public class MessageQueueService(ILogger<MessageQueueService> logger, IRepository<Message> repository, TemplateDir templateDir) : IMessageQueueService
{
    private readonly ILogger<MessageQueueService> logger = logger;
    private readonly TemplateDir templateDir = templateDir;

    public async Task<Result<Message>> TryQueueEmailAsync(Message message)
    {
        logger.LogDebug("Validating email message before adding to the queue : {message}", message);

        if (!MailAddress.TryCreate(message.RecipientContact, out MailAddress mailAddress)) return InvalidEmailError.FromEmail(message.RecipientContact);

        logger.LogDebug("Now queuing email with recipient address : {mailAddress}", mailAddress);

        await repository.AddAsync(message);

        await repository.SaveChanges();

        return Result<Message>.Success(message);
    }

    public async Task<Result<Message>> SendInviteEmailAsync(
        string inviterName,
        string inviteeEmail,
        string organizationName,
        DateTimeOffset expiryDate,
        Guid inviteLink)
    {
        string rawTemplate;
        try
        {
            rawTemplate = await GetTemplate(EmailConstants.inviteEmail);
        }
        catch (FileNotFoundException ex)
        {
            logger.LogError("{trace}",ex.StackTrace);
            return Result<Message>.Failure(new Error(ex.Message));

        }
        string replacedTemplate = rawTemplate
        .Replace("{{INVITER_NAME}}", inviterName)
        .Replace("{{ORGANIZATION_NAME}}", organizationName)
        .Replace("{{INVITE_LINK}}", inviteLink.ToString())
        .Replace("{{EXPIRY_DATE}}", expiryDate.ToString("dd/M/yyyy hh:mm"));
        

        Message inviteEmail = Message.CreateEmail(inviteeEmail, $"You've received to join {organizationName}", replacedTemplate);

        var result = await TryQueueEmailAsync(inviteEmail);

        return result;
    }

    private Task<string> GetTemplate(string templateName)
    {
        string path = templateDir.Path;
        path = Path.Combine(path, $"{templateName}");
        return File.ReadAllTextAsync(path, Encoding.UTF8);
    }
    public Task<Message> TryQueueSMS(Message message)
    {
        //validations for an SMS
        throw new NotImplementedException();
    }
}
