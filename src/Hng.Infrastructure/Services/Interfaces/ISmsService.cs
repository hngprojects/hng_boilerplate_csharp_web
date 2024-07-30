using Hng.Domain.Entities;

namespace Hng.Infrastructure.Services.Interfaces;

public interface ISmsService
{
    public Task<Message> SendSMSMessage(Message message);
}
