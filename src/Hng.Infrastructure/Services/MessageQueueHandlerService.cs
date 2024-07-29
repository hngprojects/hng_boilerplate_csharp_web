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
        timer.Dispose();
        Task.WhenAny(Task.Delay(-1, CancellationToken.None));
    }

    public Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Email service running");
        timer = new Timer(ProcessQueue, null, TimeSpan.FromSeconds(30), TimeSpan.FromMinutes(1));
        return Task.CompletedTask;
    }

    public async Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogInformation("Email service is stopping");
        timer.Change(Timeout.Infinite, 0);
        await timer.DisposeAsync();
    }

    private async void ProcessQueue(object data)
    {

        using IServiceScope scope = serviceProvider.CreateScope();

        IRepository<Message> repository = scope.ServiceProvider.GetRequiredService<IRepository<Message>>();

        logger.LogInformation("Getting message backlog");

        IEnumerable<Message> pendingMessages = await repository.GetAllBySpec(e => e.Status == Domain.Enums.MessageStatus.Pending);

        logger.LogInformation("Processing message backlog");

        foreach (var message in pendingMessages)
        {
            try
            {
                logger.LogInformation("Sending message {0} \n{1}", message.ToString(), message.Status);
                Message sentMessage = await ProcessMessage(message);
            }

            catch (Exception ex)
            {
                logger.LogInformation("Message failed to send with error {0}", ex);

                message.RetryCount += 1;

                if (message.RetryCount >= 3)
                {
                    message.Status = Domain.Enums.MessageStatus.Failed;
                }
            }
        }
        await repository.SaveChanges();
    }


    private async Task<Message> ProcessMessage(Message message)
    {
        using IServiceScope service = serviceProvider.CreateScope();
        IEmailService emailService = service.ServiceProvider.GetRequiredService<IEmailService>();
        if (message.Type == Domain.Enums.MessageType.Email) return await emailService.SendEmailMessage(message);
        return message;
        // return await _smsService.SendSMSMessage(message);
    }
}
