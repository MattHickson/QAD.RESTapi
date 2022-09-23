using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using POSAPI.Models;
namespace POSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CustomerController : ControllerBase
    {
        private readonly CustomerContext context;

        public CustomerController(CustomerContext context)
        {
            this.context = context;

        }

        [HttpGet]
        public async Task<ActionResult<Customer>> Get()
        {
            return Ok(await this.context.Customers.ToListAsync());
        }
        [HttpPost]
        public async Task<ActionResult<List<Customer>>> AddItem(Customer addCustomer)
        {
            this.context.Customers.Add(addCustomer);
            await this.context.SaveChangesAsync();
            return Ok(await this.context.Customers.ToListAsync());

        }
    }
}
