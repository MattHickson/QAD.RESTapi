using Microsoft.AspNetCore.Mvc;
using POSAPI.Models;
using Project.Models;
using POSAPI.Models;

namespace POSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ReceiptController : Controller
    {
        private readonly ReceiptContext context;
        public ReceiptController(ReceiptContext context)
        {
            this.context = context;
        }


        [HttpGet]
        public async Task<ActionResult<List<Receipt>>> Get()
        {
            return Ok(await this.context.Receipts.ToListAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Receipt>> Get(int id)
        {
            var item = await this.context.Receipts.FindAsync(id);
            if (item == null)
                return BadRequest("Bad ID");
            return Ok(item);
        }
        [HttpPost]
        public async Task<ActionResult<List<Receipt>>> AddItem(Receipt addedItem)
        {
            this.context.Receipts.Add(addedItem);
            await this.context.SaveChangesAsync();
            return Ok(await this.context.Receipts.ToListAsync());
        }
    }
}
