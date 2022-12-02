using System.ComponentModel.DataAnnotations;

namespace POSAPI.Models
{
    public class Skills
    {
        [Key]
        public int Id { get; set; }
        [Required]
        public string Name { get; set; }

        public string Tags { get; set; }
        
    }
}
