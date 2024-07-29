using Hng.Domain.Entities;
using Hng.Infrastructure.Services.Interfaces;
using Hng.Infrastructure.Utilities;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Logging;
using MimeKit;


namespace Hng.Infrastructure.Services;

public class EmailService(SmtpCredentials smtpCredentials, ILogger<EmailService> logger) : IEmailService
{
    private readonly SmtpCredentials smtpDetails = smtpCredentials;
    private readonly ILogger<EmailService> logger = logger;

    public async Task<Message> SendEmailMessage(Message message)
    {
        logger.LogInformation("Sending email message from the email service");
        logger.LogInformation(smtpDetails.Host + "\n" + smtpDetails.Port + "\n" + "JAGAGA");
        MimeMessage emailMessage = new();
        emailMessage.From.Add(new MailboxAddress("Boilerplate", "boilerplate@email.com"));
        emailMessage.To.Add(new MailboxAddress("Ace", "dummyinjustice2@gmail.com"));
        emailMessage.Subject = "Subjectify";

        emailMessage.Body = new BodyBuilder() { TextBody = message.Content }.ToMessageBody();

        using var client = new SmtpClient();

        await client.ConnectAsync(smtpDetails.Host, smtpDetails.Port, SecureSocketOptions.Auto);
        await client.AuthenticateAsync(smtpDetails.Username, smtpDetails.Password);
        await client.SendAsync(emailMessage);
        await client.DisconnectAsync(true);

        return message;
    }
}
