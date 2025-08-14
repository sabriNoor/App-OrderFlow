using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Threading.Tasks;
using App.Consumer.Models;
using App.Consumer.Services.Interfaces;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;

using Newtonsoft.Json;

namespace App.Consumer.Services
{
    public class MailService: IMessageHandler
    {
        private readonly ILogger<MailService> _logger;
        private readonly EmailConfiguration _emailConfiguration;
        public MailService(ILogger<MailService> logger, EmailConfiguration emailConfiguration)
        {
            _logger = logger;
            _emailConfiguration = emailConfiguration;

        }
        public async Task HandleMessageAsync(string message)
        {
            try
            {
                var dto = JsonConvert.DeserializeObject<IEmailMessage>(message);
                if (dto == null)
                {
                    _logger.LogWarning("Received null or invalid message: {Message}", message);
                    return;
                }

                var emailMessage = new MimeMessage();
                emailMessage.From.Add(MailboxAddress.Parse(_emailConfiguration.From));
                emailMessage.To.Add(MailboxAddress.Parse(dto.Email));
                emailMessage.Subject = dto.Subject;
                emailMessage.Body = new TextPart("plain")
                {
                    Text = dto.Body
                };

                using var smtp = new MailKit.Net.Smtp.SmtpClient();
                await smtp.ConnectAsync(_emailConfiguration.SmtpServer, _emailConfiguration.Port, SecureSocketOptions.Auto);
                await smtp.AuthenticateAsync(_emailConfiguration.UserName, _emailConfiguration.Password);
                await smtp.SendAsync(emailMessage);
                await smtp.DisconnectAsync(true);

                _logger.LogInformation("Email sent successfully to {Email}", dto.Email);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error sending email for message: {Message}", message);
            }
        }
    }
}