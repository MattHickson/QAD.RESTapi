using System.ComponentModel.DataAnnotations;

namespace POSAPI.Models
{
    public class Skills
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; } = string.Empty;

        public string Link { get; set; } = string.Empty;

        public string Tags { get; set; } = string.Empty;
        
    }
}
