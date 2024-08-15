using Hng.Domain.Entities;
using Hng.Infrastructure.Repository.Interface;
using Hng.Infrastructure.Services.Interfaces;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Hng.Infrastructure.Services;

internal class MessageQueueHandlerService(ILogger<MessageQueueHandlerService> logger, IServiceProvider serviceProvider) : IHostedService, IDisposable
{
    private readonly ILogger<MessageQueueHandlerService> logger = logger;
    private readonly IServiceProvider serviceProvider = serviceProvider;
    private Timer timer = null;

    public void Dispose()
    {
        timer?.Dispose();
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Email service running");
        timer = new Timer(ProcessQueue, null, TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Email service is stopping");
        timer.Change(Timeout.Infinite, 0);
        await timer.DisposeAsync();
    }

    private async void ProcessQueue(object data)
    {

        using IServiceScope scope = serviceProvider.CreateScope();

        IRepository<Message> repository = scope.ServiceProvider.GetRequiredService<IRepository<Message>>();

        logger.LogDebug("Getting message backlog");

        IEnumerable<Message> pendingMessages = await repository.GetAllBySpec(e => e.Status == Domain.Enums.MessageStatus.Pending);

        logger.LogDebug("Processing message backlog");

        foreach (var message in pendingMessages)
        {
            try
            {
                logger.LogDebug("Sending message to {recipientName} with contact \n{recipientContact}",
                    message.RecipientName.Replace(Environment.NewLine, ""),
                    message.RecipientContact.Replace(Environment.NewLine, ""));
                Message sentMessage = await ProcessMessage(message);
                sentMessage.Status = Domain.Enums.MessageStatus.Sent;
                await repository.UpdateAsync(sentMessage);
            }

            catch (Exception ex)
            {
                logger.LogError("Message failed to send with error {exception}", ex);

                message.RetryCount += 1;

                if (message.RetryCount >= 3)
                {
                    message.Status = Domain.Enums.MessageStatus.Failed;
                }
                await repository.UpdateAsync(message);
            }
        }
        await repository.SaveChanges();
    }


    private async Task<Message> ProcessMessage(Message message)
    {
        using IServiceScope service = serviceProvider.CreateScope();
        IEmailService emailService = service.ServiceProvider.GetRequiredService<IEmailService>();
        return await emailService.SendEmailMessage(message);
    }
}
