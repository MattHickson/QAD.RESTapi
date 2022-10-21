using Project.Models;
using System.ComponentModel.DataAnnotations;

namespace POSAPI.Models
{
	public class Receipt
	{
		[Key]
		public int Id { get; set; }
		[Required]
		public int customerId { get; set; }
		[Required]
		public string CustomerName { get; set; }
		[Required]
		
		public string items { get; set; } =	string.Empty;
		[Required]
		public double total { get; set; }

		[Required]
		public DateTime DateTime { get; set; }
	}
}
