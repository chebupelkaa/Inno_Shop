using System.ComponentModel.DataAnnotations;

namespace UserService.Application.Options
{
    public class EmailConfigurationOptions
    {
        public const string SectionName = "EmailConfiguration";

        [Required]
        [EmailAddress]
        public string From { get; set; } = string.Empty;

        [Required]
        public string SmtpServer { get; set; } = string.Empty;

        [Range(1, 65535)]
        public int Port { get; set; } = 587;

        [Required]
        public string Username { get; set; } = string.Empty;

        [Required]
        public string Password { get; set; } = string.Empty;
    }
}
