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
using static System.Net.Mime.MediaTypeNames;

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
        public int quantity { get; set; }

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

            var test = CheckCustomer(customer);
            if (test)
            {
                loginCookie();
            }
            if (!test)
            {
                using (var client = new HttpClient())
                {
                    var targeturi = "https://localhost:7148/api/Customer";

                    var Sender = new Uri(targeturi);
                    var Payload = client.PostAsJsonAsync<Customer>(Sender, customer).Result;

                    client.Dispose();
                   
                }
                CheckCustomer(customer);
                loginCookie();
            }
            

        }
        private bool CheckCustomer(Customer customer)
        {
            using (var client = new HttpClient())
            {
                var targeturi = "https://localhost:7148/api/Customer";
                var Sender = new Uri(targeturi);
                var List = client.GetFromJsonAsync<List<Customer>>(Sender).Result;
                client.Dispose();
                if (List.Count == 0)
                {
                    return false;
                }
                return customerListScroll(List, customer);
            }
        }

        private bool customerListScroll(List<Customer> customers, Customer customer)
        {
            bool value = false;
            for (int count = 0; count <= customers.Count; count++)
            {
                if (customer.CustomerName == customers[customers.Count - 1].CustomerName & customer.CustomerPassword == customers[customers.Count - 1].CustomerPassword)
                {
                    this.Customer = customers[customers.Count - 1];
                    return value = true;
                }
            }
            return value;
        }
        private void loginCookie()
        {
            string data = Customer.CustomerId.ToString();

            var cookieOptions = new CookieOptions
            {
                Secure = true,
                Expires = DateTime.Now.AddDays(1),
                HttpOnly = true,
                SameSite = SameSiteMode.Lax


            };
            Response.Cookies.Append("LoginID", data);
        }
    }
}
