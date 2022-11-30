using Microsoft.AspNetCore.Mvc;
using POSAPI.Models;

namespace POSPages.Controllers
{
    public class LogicController : Controller
    {

        
        
        public bool CheckCustomer(Customer customer)
        {
            using (var client = new HttpClient())
            {
                var targeturi = "https://poswebapiservice.azurewebsites.net/api/Customer";
                var Sender = new Uri(targeturi);
                var List = client.GetFromJsonAsync<List<Customer>>(Sender).Result;
                client.Dispose();
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
                    return value = true;
                }
            }
            return value;
        }
    }
}
