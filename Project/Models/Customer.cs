using System.ComponentModel.DataAnnotations;

namespace POSAPI.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        [Required]
        public string CustomerPassword { get; set; } = string.Empty;
    }
}
