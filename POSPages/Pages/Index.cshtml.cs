using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSAPI.Models;
using Project.Models;

namespace POSPages.Pages
{
    public class IndexModel : PageModel
    {

        [BindProperty]
        public Customer customer { get; set; } = default!;
      
        
        


        public void OnGet()
        {

            Customer customer = new Customer();
            
        }


        public async Task<IActionResult> OnPostAsync()
        {
            customer.CustomerId = 0;
            if (!ModelState.IsValid || customer == null || customer == null)
            {
                return Page();
            }


            createCustomer(customer);
            return RedirectToPage("./Menu");
        }
        public void createCustomer(Customer customer)
        {
            using (var client = new HttpClient())
            {
                var targeturi = "https://localhost:7148/api/Customer";

                var Sender = new Uri(targeturi);
                var Payload = client.PostAsJsonAsync<Customer>(Sender, customer);


                client.Dispose();

            }
        }
    }
}