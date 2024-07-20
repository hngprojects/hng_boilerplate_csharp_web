using Hng.Application.Interfaces;
using MailKit.Net.Smtp;
using MimeKit;
using System;
using System.Threading.Tasks;

namespace Hng.Application.Services
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly string _smtpUser;
        private readonly string _smtpPass;


        public EmailService(string smtpServer, string smtpUser, string smtpPass)
        {
            _smtpServer = smtpServer;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
        }

        public async Task SendEmailAsync(string to, string subject, string body)
        {
            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(_smtpUser, _smtpUser));
            message.To.Add(new MailboxAddress(to, to));
            message.Subject = subject;

            message.Body = new TextPart("html")
            {
                Text = body
            };

            using var client = new SmtpClient();
            try
            {
                await client.ConnectAsync(_smtpServer,465, true);
                await client.AuthenticateAsync(_smtpUser, _smtpPass);

                await client.SendAsync(message);
                await client.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                // Log the detailed error
                Console.WriteLine($"Email sending failed: {ex}");
                throw;
            }
        }
    }
}
