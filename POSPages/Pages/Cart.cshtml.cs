using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSAPI.Migrations;
using POSAPI.Models;
using Project.Models;

namespace POSPages.Pages
{
    public class CartModel : PageModel
    {


        public List<Cart> Items { get; set; }
        private Customer Customer { get; set; }
        public double Total { get; set; }

        public async Task<IActionResult> OnGetAsync()
        {

            bool loginQuestion = CheckCustomer();
            if (loginQuestion)
            {
                CartGrab();
                totalout();

                 return Page();
            }
            else
            {
                return RedirectToPage("./Login");
            }

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
            this.Customer = customer;
        }

        private void CartGrab()
        {

            using (var client = new HttpClient())
            {
                
                var targeturi = "https://localhost:7148/api/Cart";
                var Sender = new Uri(targeturi);
                var Cart = client.GetFromJsonAsync<List<Cart>>(Sender).Result;
                client.Dispose();
                List<Cart> finalCart = new List<Cart>();
                for (int count = 0; count <= Cart.Count-1; count++)
                {
                    if (Cart[count].CustomerId == this.Customer.CustomerId)
                    {
                        finalCart.Add(Cart[count]);
                    }
                }
                this.Items = finalCart;
            }
        }
        private void totalout()
        {
            for (int count = 0; count <= Items.Count - 1; count++)
            {
                this.Total = Total + (Items[count].Price * Items[count].Quantity);
            }

        }
        
    }
}
