using Hng.Domain.Entities;

namespace Hng.Infrastructure.Services.Interfaces;

internal interface IEmailService
{

    public Task<Message> SendEmailMessage(Message message);
}
