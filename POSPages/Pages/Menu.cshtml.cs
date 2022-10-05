using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Data;
using Project.Models;
using Project.Controllers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Linq;
using POSAPI.Models;
namespace POSPages.Pages
{
    public class MenuModel : PageModel
    {
        
       
        public IEnumerable<Item> Items { get; set; }

        
       public MenuModel()
        {
            using (var client = new HttpClient())
            {
                var request = new Uri("https://localhost:7148/api/Item");
                var response = client.GetFromJsonAsync<IEnumerable<Item>>(request).Result;
                Items = response;
                client.Dispose();
            }
        }

        public async Task<IActionResult> OnGetAsync()
        {

            bool loginQuestion = CheckCustomer();
                if (loginQuestion)
                {
                    

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
                    if (customers[customers.Count - 1].CustomerId == id)
                    {

                        return true;
                    }
                }
            }

            return false;
        }
    }
 }

