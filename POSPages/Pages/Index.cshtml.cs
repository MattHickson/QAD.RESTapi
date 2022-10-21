using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using POSAPI.Models;
using Project.Models;

namespace POSPages.Pages
{
    public class IndexModel : PageModel
    {
        private Customer customer { get; set; }
        [BindProperty]
        public bool logged { get; set; }
        public string customerName { get; set; }
        public void OnGet()
        {
            this.logged = CheckCustomer();
            if (logged)
            {
                this.customerName = customer.CustomerName;
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
            var NullTest = Request.Cookies["LoginID"];
            if (NullTest == null)
            {
                return false;
            }
            int id = int.Parse(Request.Cookies["LoginID"]);
            if (id != null)
            {

                for (int count = 0; count <= customers.Count - 1; count++)
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