using System.ComponentModel.DataAnnotations;

namespace ProductService.Application.Options
{
    public class ConnectionStringsOptions
    {
        public const string SectionName = "ConnectionStrings";

        [Required]
        public string ProductsDb { get; set; } = string.Empty;
    }
}
