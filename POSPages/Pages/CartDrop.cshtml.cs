using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Microsoft.EntityFrameworkCore;
using Project.Data;
using System.Net.Http.Json;
using POSPages.Models;
using static System.Net.Mime.MediaTypeNames;

namespace POSPages.Pages
{
    public class CartDropModel : PageModel
    {


        [BindProperty]
        public Item Item { get; set; } = default!;
        [BindProperty]
        public int Quant { get; set; }
       
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

                var targeturi = "https://poswebapiservice.azurewebsites.net/api/Item/" + id;
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
            buildCart();
            if (cartCheck())
            {
                int cartId = CartId();
                if (cartId != -1)
                using (var client = new HttpClient())
                {
                    var targeturi = "https://poswebapiservice.azurewebsites.net/api/Cart";
                    var Sender = new Uri(targeturi);
                    var payload = client.PutAsJsonAsync<Cart>(Sender, this.Cart).Result;
                    client.Dispose();
                }
            }
            else
            {
                using (var client = new HttpClient())
                {
                    var targeturi = "https://poswebapiservice.azurewebsites.net/api/Cart";
                    var Sender = new Uri(targeturi);
                    var payload = client.PostAsJsonAsync<Cart>(Sender, this.Cart).Result;
                    client.Dispose();
                }
            }
        }
        //Builds a cart ready for post
        private void buildCart()
        {

            var item = ItemGrab(int.Parse(Request.Cookies["CurrentCart"]));
            this.Cart.Id = 0;
            this.Cart.Name = item.Name;
            this.Cart.Price = Math.Round(item.Price,2);
            this.Cart.Quantity = 1;
            this.Cart.CustomerId = int.Parse(Request.Cookies["LoginID"]);
            
           
            this.Cart.Quantity = int.Parse(Request.Form["Quant"]);
          

        }
        //Grabs the corrected Cart ID for posting
        private int CartId()
        {
            using (var client = new HttpClient())
            {


                var targeturi = "https://poswebapiservice.azurewebsites.net/api/Cart";
                var Sender = new Uri(targeturi);
                var test = client.GetFromJsonAsync<List<Cart>>(Sender).Result;
                client.Dispose();
                for (int count = 0; count <= test.Count - 1; count++)
                {
                    if (test[count].CustomerId == int.Parse(Request.Cookies["LoginID"]) && test[count].Name == this.Cart.Name)
                    {
                        this.Cart.Id = test[count].Id;
                        return test[count].Id;
                    }
                }
            }
            return -1;
        }
        //todo Add checks to see if customer has Item in cart 
        // If item exists(true) then update with quntity + new quantity
        private bool cartCheck()
        {
            
            using (var client = new HttpClient())
            {

                
                var targeturi = "https://poswebapiservice.azurewebsites.net/api/Cart";
                var Sender = new Uri(targeturi);
                var test = client.GetFromJsonAsync<List<Cart>>(Sender).Result;
                client.Dispose();
                if (test != null)
                {
                    for (int count = 0; count <= test.Count-1 ;count++)
                    {
                        if(test[count].CustomerId == int.Parse(Request.Cookies["LoginID"]) && test[count].Name == this.Cart.Name)
                        {
                            
                            return true;
                        }
                    }
                }
            }
            
            return false;
        }

        private bool CheckCustomer()
        {
            using (var client = new HttpClient())
            {
                var targeturi = "https://poswebapiservice.azurewebsites.net/api/Customer";
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

                for (int count = 0; count <= customers.Count-1; count++)
                {
                    if (customers[count].CustomerId == id)
                    {
                        buildcustomer(customers[count]);
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
