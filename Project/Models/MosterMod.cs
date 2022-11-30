using System.ComponentModel.DataAnnotations;

namespace POSAPI.Models
{
    public class MonsterMod
    {
        [Required]
        [Key]
        public int Id { get; set; }
        [Required]
        public string ModName { get; set; } = String.Empty;
        [Required]
        public string modEffects { get; set; } = String.Empty;
    }
}
