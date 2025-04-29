using System.ComponentModel.DataAnnotations;
using RealEstateApp.Models;


namespace RealEstateApp.Models
{
    public enum PropertyType
    {
        ForRent,
        ForSale
    }

    public class House
    {
        [Key]
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public string Title { get; set; }

        [Required]
        public string Description { get; set; }

        [Required]
        public decimal Price { get; set; }

        [Required]
        public PropertyCategory Category { get; set; }

        public PropertyType PropertyType { get; set; }

        [Required]
        public string ImageUrl { get; set; }

        public Guid AgentId { get; set; }

        public ApplicationUser? Agent { get; set; }
    }
}
