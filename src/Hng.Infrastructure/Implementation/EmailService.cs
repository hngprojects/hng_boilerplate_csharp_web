using Hng.Application.Models.EmailModels;
using Hng.Application.Services;
using Microsoft.Extensions.Options;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace Hng.Infrastructure.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly string _smtpServer;
        private readonly int _smtpPort;
        private readonly string _smtpUser;
        private readonly string _smtpPass;

        public EmailService(string smtpServer, int smtpPort, string smtpUser, string smtpPass)
        {
            _smtpServer = smtpServer;
            _smtpPort = smtpPort;
            _smtpUser = smtpUser;
            _smtpPass = smtpPass;
        }

        public async Task<bool> SendEmailAsync(string to, string subject, string body)
        {
            if (string.IsNullOrEmpty(to)) throw new ArgumentNullException(nameof(to), "Email 'To' address cannot be null or empty.");
            if (string.IsNullOrEmpty(subject)) throw new ArgumentNullException(nameof(subject), "Email 'Subject' cannot be null or empty.");
            if (string.IsNullOrEmpty(body)) throw new ArgumentNullException(nameof(body), "Email 'Body' cannot be null or empty.");

            var mail = new MailMessage();
            mail.To.Add(new MailAddress(to));
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new MailAddress(_smtpUser);

            using var smtp = new SmtpClient(_smtpServer, _smtpPort)
            {
                Credentials = new NetworkCredential(_smtpUser, _smtpPass),
                EnableSsl = false
            };

            try
            {
                await smtp.SendMailAsync(mail);
                return true;
            }
            catch (Exception ex)
            {
                throw new InvalidOperationException("Failed to send email.", ex);
            }
        }
    }
}
