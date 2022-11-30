using System.ComponentModel.DataAnnotations;

namespace POSPages.Models
{
    public class Customer
    {
        [Key]
        public int CustomerId { get; set; }

        [RegularExpression(@"^[a-zA-Z1-9''-'\s]{4,20}$",
         ErrorMessage = "Username must be 4 - 20 Characters long.")]
        [Required]
        public string CustomerName { get; set; } = string.Empty;
        [RegularExpression(@"^[a-zA-Z1-9''-'\s]{4,32}$",
         ErrorMessage = "Password must be 4-32 Long with Only letters and Numbers.")]
        [Required]
        public string CustomerPassword { get; set; } = string.Empty;
    }
}
