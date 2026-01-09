using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MimeKit.Text;
using UserService.Application.Interfaces;

namespace UserService.Infrastructure.Services
{
    public class EmailService : IEmailService
    {
        private readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }


        public async Task SendPasswordRecovery(string email, string token)
        {
            var resetLink = $"{_configuration["AppUrl"]}/reset-password?token={token}&email={email}";

            var htmlBody = $@"<a href='{$"{resetLink}"}'>Click here to reset password</a>";

            await SendEmailAsync(email, "Reset your password from InnoShop", htmlBody);
        }


        public async Task SendEmailConfirmation(string email, string userName, string token)
        {
            var link = $"{_configuration["AppUrl"]}/confirm-email?token={token}&email={Uri.EscapeDataString(email)}";

            var htmlBody = $@"<a href='{$"{link}"}'>Click here to confirm email</a>";

            await SendEmailAsync(email, "Confirm your InnoShop account", htmlBody);
        }


        public async Task SendEmailAsync(string email, string subject, string htmlBody)
        {
            var message = new MimeMessage();

            message.From.Add(new MailboxAddress("InnoShop", _configuration["EmailConfiguration:From"]));
            message.To.Add(new MailboxAddress(email, email));
            message.Subject = subject;
            message.Body = new TextPart(TextFormat.Html) { Text = htmlBody };

            using var smtp = new SmtpClient();

            smtp.ServerCertificateValidationCallback = (s, c, h, e) => true;

            await smtp.ConnectAsync(_configuration["EmailConfiguration:SmtpServer"], Convert.ToInt32(_configuration["EmailConfiguration:Port"]),
                MailKit.Security.SecureSocketOptions.StartTls);

            await smtp.AuthenticateAsync(_configuration["EmailConfiguration:Username"], _configuration["EmailConfiguration:Password"]);

            await smtp.SendAsync(message);
            await smtp.DisconnectAsync(true);
        }
    }
}
