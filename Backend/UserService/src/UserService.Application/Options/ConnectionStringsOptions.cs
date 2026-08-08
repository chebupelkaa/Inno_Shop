using System.ComponentModel.DataAnnotations;

namespace UserService.Application.Options
{
    public class ConnectionStringsOptions
    {
        public const string SectionName = "ConnectionStrings";

        [Required]
        public string UsersDb { get; set; } = string.Empty;
    }
}
