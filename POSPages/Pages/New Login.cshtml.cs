using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.AspNetCore.Mvc.Rendering;
using POSAPI.Models;
using Project.Data;
using Project.Models;
using POSPages.Controllers;

namespace POSPages.Pages
{
    public class NewLoginModel : PageModel
    {
 /*       private readonly CustomerContext _context;

        public CreateModel(CustomerContext context)
        {
            _context = context;
        }
 */
        public IActionResult OnGet()
        {
            return Page();
        }

        [BindProperty]
        public Customer Customer { get; set; } = default!;
        

        // To protect from overposting attacks, see https://aka.ms/RazorPagesCRUD
        public async Task<IActionResult> OnPostAsync()
        {
          if (!ModelState.IsValid || Customer == null)
            {
                return Page();
            }


          
          createCustomer(Customer);

            return RedirectToPage("./Menu");
        }

        //only ever used on this page
        private void createCustomer(Customer customer)
        {
            using (var client = new HttpClient())
            {
                var targeturi = "https://localhost:7148/api/Customer";

                var Sender = new Uri(targeturi);
                var Payload = client.PostAsJsonAsync<Customer>(Sender, customer).Result;


                client.Dispose();
                
            }
        }
    }
}
