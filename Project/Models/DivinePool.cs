using System.ComponentModel.DataAnnotations;

namespace POSAPI.Models
{
    public class DivinePool
    {
        [Key]
        public int id { get; set; }
        public int amount { get; set; }
    }
}
