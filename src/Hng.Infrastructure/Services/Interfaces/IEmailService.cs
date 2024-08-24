using Hng.Domain.Entities;

namespace Hng.Infrastructure.Services.Interfaces;

public interface IEmailService
{

    public Task<Message> SendEmailMessage(Message message);
}
