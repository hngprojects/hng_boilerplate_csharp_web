using Hng.Domain.Entities;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Infrastructure.Utilities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;


namespace Hng.Infrastructure.Services.Internal;

internal class EmailService(SmtpCredentials smtpCredentials, ILogger<EmailService> logger) : IEmailService
{
    private readonly SmtpCredentials smtpDetails = smtpCredentials;
    private readonly ILogger<EmailService> logger = logger;

    public async Task<Message> SendEmailMessage(Message message)
    {
        logger.LogDebug("Sending the passed email message from the email service");
        MimeMessage emailMessage = new();
        emailMessage.From.Add(new MailboxAddress("HNG Boilerplate", "boilerplate@email.com"));
        emailMessage.To.Add(new MailboxAddress($"{message.RecipientName}", $"{message.RecipientContact}"));
        emailMessage.Subject = message.Subject;

        emailMessage.Body = new BodyBuilder() { TextBody = message.Content }.ToMessageBody();

        using var client = new SmtpClient();

        await client.ConnectAsync(smtpDetails.Host, smtpDetails.Port, SecureSocketOptions.Auto);
        await client.AuthenticateAsync(smtpDetails.Username, smtpDetails.Password);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);

        return message;

    }
}
