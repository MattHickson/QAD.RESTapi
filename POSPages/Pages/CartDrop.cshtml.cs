using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using System.Net.Http.Json;

namespace POSPages.Pages
{
    public class CartDropModel : PageModel
    {

      
        [BindProperty]
        public Item Item { get; set; } = default!;
       
        
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            using (var client = new HttpClient())
            {
                var targeturi = "https://localhost:7148/api/Item/" + id.ToString();
                var request = new Uri(targeturi);
                var response = client.GetFromJsonAsync<Item>(request).Result;
                Item = response;
                client.Dispose();
                return Page();
            }
            
        }
        public void cartPost()
        {
            using (var client = new HttpClient())
            {
                var targeturi = "https://localhost:7148/api/Cart";

                var Sender = new Uri(targeturi);
                

                client.Dispose();
            }

            
        }
       
    }
}
