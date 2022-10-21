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
        public Receipt receipt { get; set; }
        public void OnGet()
        {
            this.logged = CheckCustomer();
            if (logged)
            {
                this.customerName = customer.CustomerName;

                if (lastReceipt())
                {

                }
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
        private bool lastReceipt()
        {
            int lastCheck = -1;
            
            List<Receipt> receiptList = new List<Receipt>();
            using (var client = new HttpClient())
            {

                var targeturi = "https://localhost:7148/api/Receipt";
                var Sender = new Uri(targeturi);
                var List = client.GetFromJsonAsync<List<Receipt>>(Sender).Result;
                client.Dispose();
                for(int count = 0; count <= List.Count - 1; count++)
                {
                    if (List[count].customerId == customer.CustomerId)
                    {
                        receiptList.Add(List[count]);
                    }
                }
                if(receiptList.Count > 0)
                {
                    if(receiptList.Count == 1)
                    {
                        this.receipt = receiptList[0];
                        return true;
                    }
                    for(int count = 0; count <= receiptList.Count - 1; count++)
                    {
                        if (lastCheck == -1)
                        {
                            
                            this.receipt = receiptList[count];
                        }
                       if(lastCheck == 1)
                        {
                            if (DateTime.Compare(receiptList[count].DateTime, this.receipt.DateTime) == -1)
                            {
                                
                                this.receipt = receiptList[count];
                            }
                        }
                        lastCheck = 1;
                        
                    }
                    return true;
                }
                return false;
            }
        }
    }
}