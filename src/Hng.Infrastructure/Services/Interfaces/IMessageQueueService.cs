using Hng.Domain.Entities;

namespace Hng.Infrastructure.Services.Interfaces;

public interface IMessageQueueService
{
    public Task<Message> TryQueueEmailAsync(Message message);
    public Task<Message> TryQueueSMS(Message message);
}
