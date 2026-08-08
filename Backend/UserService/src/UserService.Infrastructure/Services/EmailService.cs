using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.Options;
using MimeKit;
using MimeKit.Text;
using UserService.Application.Interfaces;
using UserService.Application.Options;

namespace UserService.Infrastructure.Services
{
    public class EmailService(IOptions<EmailConfigurationOptions> emailOptions, IConfiguration configuration) : IEmailService
    {
        private readonly EmailConfigurationOptions _emailOptions = emailOptions.Value;

        public async Task SendPasswordRecovery(string email, string token)
        {
            var resetLink = $"{configuration["AppUrl"]}/reset-password?token={token}&email={email}";
            var htmlBody = $@"<a href='{resetLink}'>Click here to reset password</a>";

            await SendEmailAsync(email, "Reset your password from InnoShop", htmlBody);
        }

        public async Task SendEmailConfirmation(string email, string userName, string token)
        {
            var link = $"{configuration["AppUrl"]}/confirm-email?token={token}&email={Uri.EscapeDataString(email)}";
            var htmlBody = $@"<a href='{link}'>Click here to confirm email</a>";

            await SendEmailAsync(email, "Confirm your InnoShop account", htmlBody);
        }

        public async Task SendEmailAsync(string email, string subject, string htmlBody)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("InnoShop", _emailOptions.From));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html) { Text = htmlBody };

            using var smtp = new SmtpClient();

            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

            await smtp.ConnectAsync(_emailOptions.SmtpServer, _emailOptions.Port,
                MailKit.Security.SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(_emailOptions.Username, _emailOptions.Password);

            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
