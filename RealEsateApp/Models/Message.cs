using System.ComponentModel.DataAnnotations;

namespace RealEstateApp.Models
{
    public class Message
    {
        public Guid Id { get; set; } = Guid.NewGuid();

        [Required]
        public Guid SenderId { get; set; }

        [Required]
        public Guid ReceiverId { get; set; }

        [Required]
        [StringLength(100)]
        public string Subject { get; set; } = string.Empty;

        [Required]
        [StringLength(1000)]
        public string Content { get; set; } = string.Empty;

        public DateTime SentAt { get; set; }

        //public DateTime SentOn { get; set; }

        public ApplicationUser? Sender { get; set; }

        public ApplicationUser? Receiver { get; set; }
    }
}
