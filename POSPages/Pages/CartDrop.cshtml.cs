using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using Project.Models;
using System.Net.Http.Json;
using POSAPI.Models;

namespace POSPages.Pages
{
    public class CartDropModel : PageModel
    {


        [BindProperty]
        public Item Item { get; set; } = default!;
        public Cart Cart = new Cart();
        private int id;
        private Customer customer { get; set; }
        public async Task<IActionResult> OnGetAsync(int? id)
        {
            if (id != null)
            {
                if (CheckCustomer())
                {
                    this.Item = ItemGrab((int)id);
                    this.id = (int)id;
                    string data = this.id.ToString();

                    var cookieOptions = new CookieOptions
                    {
                        Secure = true,
                        Expires = DateTime.Now.AddDays(1),
                        HttpOnly = true,
                        SameSite = SameSiteMode.Lax


                    };
                    Response.Cookies.Append("CurrentCart", data);
                    return Page();
                }
                else
                {
                    return RedirectToPage("./Login");
                }

            }
            return RedirectToPage("./Error");
        }



        public async Task<IActionResult> OnPostAsync()
        {
            CartPayload();
            return RedirectToPage("./Menu");
        }







        //Handls API Connection for Item Grabbing
        private Item ItemGrab(int id)
        {
            using (var client = new HttpClient())
            {

                var targeturi = "https://localhost:7148/api/Item/" + id;
                var request = new Uri(targeturi);
                var response = client.GetFromJsonAsync<Item>(request).Result;
                var item = response;
                client.Dispose();
                if (item != null)
                {
                    return item;
                }
                return item;
            }

        }



        private void CartPayload()
        {

            using (var client = new HttpClient())
            {
                var targeturi = "https://localhost:7148/api/Cart";
                var Sender = new Uri(targeturi);
                //cart check
                buildCart();
                var payload = client.PostAsJsonAsync<Cart>(Sender, this.Cart).Result;

                client.Dispose();
            }
        }
        //Builds a fresh cart on the flys
        private void buildCart()
        {

            var item = ItemGrab(int.Parse(Request.Cookies["CurrentCart"]));
            this.Cart.Id = 0;
            this.Cart.Name = item.Name;
            this.Cart.Price = item.Price;
            this.Cart.Quantity = 1;
            this.Cart.CustomerId = int.Parse(Request.Cookies["LoginID"]);

        }



        //todo Add checks to see if customer has Item in cart 
        // If item exists(true) then update with quntity + new quantity
        private bool cartCheck()
        {
            bool existing = false;


            return existing;
        }

        private bool CheckCustomer()
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
                return customerListScrollId(List);
            }

        }
        private bool customerListScrollId(List<Customer> customers)
        {

            int id = int.Parse(Request.Cookies["LoginID"]);
            if (id != null)
            {

                for (int count = 0; count <= customers.Count; count++)
                {
                    if (customers[customers.Count - 1].CustomerId == id)
                    {
                        buildcustomer(customers[customers.Count - 1]);
                        return true;
                    }
                }
            }

            return false;
        }
        private void buildcustomer(Customer customer)
        {
            this.customer = customer;
        }

    }
}
