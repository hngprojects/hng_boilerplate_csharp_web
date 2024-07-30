using System.Net.Mail;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hng.Infrastructure.Services;

public class MessageQueueService(ILogger<MessageQueueService> logger, IRepository<Message> repository) : IMessageQueueService
{
    private readonly ILogger<MessageQueueService> logger = logger;

    public async Task<Message> TryQueueEmailAsync(Message message)
    {
        logger.LogDebug("Validating email message before adding to the queue : {message}", message);

        if (!MailAddress.TryCreate(message.RecipientContact, out MailAddress mailAddress)) return null;

        logger.LogDebug("Now queuing email with recipient address : {mailAddress}", mailAddress);

        await repository.AddAsync(message);

        await repository.SaveChanges();

        return message;
    }

    public Task<Message> TryQueueSMS(Message message)
    {
        //validations for an SMS
        throw new NotImplementedException();
    }
}
