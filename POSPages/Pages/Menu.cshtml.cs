using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.RazorPages;
using Project.Data;
using Project.Models;
using Project.Controllers;
using System.Net.Http.Json;
using System.Text.Json;
using System.Linq;

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
        public void OnGet()
        {
            
           
           

        }
    }
 }

