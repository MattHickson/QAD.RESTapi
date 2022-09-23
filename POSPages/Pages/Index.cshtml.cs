using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSAPI.Models;

namespace POSPages.Pages
{
    public class IndexModel : PageModel
    {
        public readonly Customer NewCustomer;

        public IndexModel(Customer newCustomer)
        {
            NewCustomer = newCustomer;
        }

        public void OnGet()
        {
            
        }
        public void onPost()
        {
            using (var client = new HttpClient())
            {
                var targeturi = "https://localhost:7148/api/Customer";

                var Sender = new Uri(targeturi);

                

                client.Dispose();
            }
        }
        public void createCustomer()
        {

        }
    }
}