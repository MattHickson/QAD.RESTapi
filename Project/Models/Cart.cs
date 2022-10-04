using POSAPI.Models;
using Project.Models;
using System.ComponentModel.DataAnnotations;

namespace Project.Models
{
    public class Cart
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = String.Empty;
        [Required]
        public double Price { get; set; }
        [Required]
        public int Quantity { get; set; }
        [Required]
        public int CustomerID;
    }
}
