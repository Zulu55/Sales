using System.ComponentModel.DataAnnotations;

namespace Sales.Common.Models
{
    using System.ComponentModel.DataAnnotations;

    public class Assistant
    {
        [Key]
        public int AssistantId { get; set; }

        [Required]
        [MaxLength(100)]
        public string FullName { get; set; }

        [Required]
        [MaxLength(20)]
        public string Locallity { get; set; }

        public string TicketId { get; set; }
    }
}
