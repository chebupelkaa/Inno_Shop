using System.ComponentModel.DataAnnotations;

namespace ProductService.Domain.Entities
{
    public class Product
    {
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }
        public string Description { get; set; }
        public decimal Price { get; set; }
        public bool Availability { get; set; } = true;
        [Required]
        public int UserId { get; set; }
        public DateTime DateOfCreation { get; set; } = DateTime.UtcNow;
        //public User User { get; set; }
    }
}
