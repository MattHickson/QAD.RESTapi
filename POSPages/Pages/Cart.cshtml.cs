using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;

using POSPages.Models;

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
        public async Task<IActionResult> OnPostAsync()
        {
            CheckCustomer();
            CartGrab();
            totalout();
            finishRecipt();

            return RedirectToPage("./Index");
        }
        public IActionResult OnPostDelete()
        {
            CheckCustomer();
            CartGrab();
            deleteCart();
            return RedirectToPage("./Cart");
        }
        //Page Check for active Customer data
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
            var idTest = Request.Cookies["LoginID"];
            if (idTest == null) { return false; }
            int id = int.Parse(Request.Cookies["LoginID"]);
            if (id != null)
            {

                for (int count = 0; count <= customers.Count-1; count++)
                {
                    if (customers[count].CustomerId == id)
                    {
                        this.Customer = customers[count];
                        return true;
                    }
                }
            }

            return false;
        }
        private void CartGrab()
        {

            using (var client = new HttpClient())
            {

                var targeturi = "https://poswebapiservice.azurewebsites.net/api/Cart";
                var Sender = new Uri(targeturi);
                var Cart = client.GetFromJsonAsync<List<Cart>>(Sender).Result;
                client.Dispose();
                List<Cart> finalCart = new List<Cart>();
                for (int count = 0; count <= Cart.Count - 1; count++)
                {
                    if (Cart[count].CustomerId == this.Customer.CustomerId)
                    {
                        finalCart.Add(Cart[count]);
                    }
                }
                this.Items = finalCart;
            }
        }
        //Gives current Total Cost of all Items * individual quantities
        private void totalout()
        {
            for (int count = 0; count <= Items.Count - 1; count++)
            {
                this.Total = Total + (Items[count].Price * Items[count].Quantity);

            }
            this.Total = Math.Round(this.Total, 2);
        }
        // Only used on Cart Page
        private void finishRecipt()
        {
            Receipt receipt = new Receipt();
            receipt.Id = 0;
            receipt.total = this.Total;
            receipt.CustomerName = this.Customer.CustomerName;
            receipt.customerId = this.Customer.CustomerId;
            receipt.DateTime = DateTime.Now;
            for (int count = 0; count <= Items.Count - 1; count++)
            {
                receipt.items += Items[count].Name + ":" + Items[count].Price.ToString() + ":" + Items[count].Quantity.ToString() + ":" + (Items[count].Price * Items[count].Quantity).ToString();
                if (count != Items.Count - 1)
                {
                    receipt.items += ":";
                }
            }
            using (var client = new HttpClient())
            {

                var targeturi = "https://poswebapiservice.azurewebsites.net/api/Receipt";
                var Sender = new Uri(targeturi);
                var payload = client.PostAsJsonAsync<Receipt>(Sender, receipt).Result;
                client.Dispose();
            }
            CartRemove();
        }
        //Remove User's Current active Cart on Recipt Create
        private void CartRemove()
        {
            for (int count = 0; count <= Items.Count - 1; count++)
            {
                //Achive into an AchiveCart here with a Date
                using (var client = new HttpClient())
                {
                    var targeturi = "https://poswebapiservice.azurewebsites.net/api/Cart?id=" + Items[count].Id;
                    var Sender = new Uri(targeturi);
                    var payload = client.DeleteAsync(Sender).Result;
                    client.Dispose();

                }
            }
        }
        private void deleteCart()
        {
            var data = Request.Form["target"];
            int target = int.Parse(data);
            using (var client = new HttpClient())
            {
                var targeturi = "https://poswebapiservice.azurewebsites.net/api/Cart?id=" + target;
                var Sender = new Uri(targeturi);
                var payload = client.DeleteAsync(Sender).Result;
                client.Dispose();

            }
        }
    }
}
