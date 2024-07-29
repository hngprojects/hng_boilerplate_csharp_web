using System.Net.Mail;
using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.Logging;

namespace Hng.Infrastructure.Services;

public class MessageQueueService(ILogger<MessageQueueService> logger, IRepository<Message> repository) : IMessageQueueService
{
    private readonly ILogger<MessageQueueService> logger = logger;

    public async Task<Message> TryQueueEmail(Message message)
    {
        logger.LogInformation("Validating email message before adding to the queue : {0}", message);

        if (!MailAddress.TryCreate(message.Recipient, out MailAddress mailAddress)) return null;

        logger.LogInformation("Now queuing email with recipient address : {0}", mailAddress);

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
