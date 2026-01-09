namespace UserService.Application.Interfaces
{
    public interface IEmailService
    {
        Task SendPasswordRecovery(string email, string token);

        Task SendEmailConfirmation(string email, string userName, string token);

        Task SendEmailAsync(string email, string subject, string htmlBody);
    }
}
