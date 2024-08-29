using System.Text.Json;
using Hng.Domain.Entities;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Infrastructure.Utilities.EmailQueue;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using StackExchange.Redis;

namespace Hng.Infrastructure.Services;

internal class MessageQueueHandlerService(ILogger<MessageQueueHandlerService> logger, IServiceProvider serviceProvider, IConnectionMultiplexer redis) : IHostedService
{
    private readonly ILogger<MessageQueueHandlerService> logger = logger;
    private readonly IServiceProvider serviceProvider = serviceProvider;
    private readonly IConnectionMultiplexer redis = redis;

    public async Task StartAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Email service running");

        ISubscriber subscriber = redis.GetSubscriber();

        await subscriber.SubscribeAsync(RedisChannel.Literal("email_queue"),
            async (channel, message) => await MessageSubscriber(channel, message, cancellationToken));

    }

    public Task StopAsync(CancellationToken cancellationToken)
    {
        logger.LogDebug("Email service is stopping");
        return Task.CompletedTask;
    }

    private async Task MessageSubscriber(RedisChannel redisChannel, RedisValue message, CancellationToken cancellationToken)
    {
        Message email = JsonSerializer.Deserialize<Email>(message).ToMessage();

        logger.LogInformation("Email subscriber processing received message for {recipient}", email.RecipientName.Replace(Environment.NewLine, ""));

        await ProcessMessage(email, cancellationToken);
    }

    private async Task ProcessMessage(Message message, CancellationToken cancellationToken)
    {
        using IServiceScope service = serviceProvider.CreateScope();
        IEmailService emailService = service.ServiceProvider.GetRequiredService<IEmailService>();

        bool sent = await Send();

        if (!sent)
        {
            logger.LogWarning("Failed to send email to {recipient}. Retrying in 1 minute...", message.RecipientContact.Replace(Environment.NewLine, ""));
            await Task.Delay(TimeSpan.FromMinutes(1), cancellationToken);
            sent = await Send();

            if (!sent)
            {
                logger.LogError("Retry failed for email to {recipient}. Message will not be processed further.", message.RecipientContact.Replace(Environment.NewLine, ""));
            }
        }

        async Task<bool> Send()
        {
            try
            {
                logger.LogInformation("Sending message to {recipientName} with contact \n{recipientContact}",
                    message.RecipientName.Replace(Environment.NewLine, ""),
                    message.RecipientContact.Replace(Environment.NewLine, ""));
                Message sentMessage = await emailService.SendEmailMessage(message);
                sentMessage.Status = Domain.Enums.MessageStatus.Sent;
                sentMessage.LastAttemptedAt = DateTimeOffset.UtcNow;
                return true;
            }
            catch (Exception ex)
            {
                logger.LogError("An error occurred when sending the mail {error}", ex);
                return false;
            }
        }

    }


}

