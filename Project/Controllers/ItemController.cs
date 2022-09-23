using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Project.Models;

namespace Project.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class ItemController : ControllerBase
    {


        private readonly ItemContext context;
        public ItemController(ItemContext context)
        {
            this.context = context;
        }

        [HttpGet]
        public async Task<ActionResult<List<Item>>> Get()
        {
            return Ok(await this.context.Items.ToListAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Item>> Get(int id)
        {
            var item = await this.context.Items.FindAsync(id);
            if (item == null)
                return BadRequest("Bad ID");
            return Ok(item);
        }
        [HttpPost]
        public async Task<ActionResult<List<Item>>> AddItem(Item addedItem)
        {
            this.context.Items.Add(addedItem);
            await this.context.SaveChangesAsync();
            return Ok(await this.context.Items.ToListAsync());
        }
        [HttpPut]
        public async Task<ActionResult<List<Item>>> UpdateItem(Item request)
        {
            var item = await this.context.Items.FindAsync(request.Id);
            if (item == null)
                return BadRequest("Bad ID");
            item.Name = request.Name;
            item.Description = request.Description;
            item.Price = request.Price;
            await this.context.SaveChangesAsync();
            return Ok(await this.context.Items.ToListAsync());
        }
        [HttpDelete]
        public async Task<ActionResult<List<Item>>> DeleteItem(int id)
        {
            var item = await this.context.Items.FindAsync(id);
            if (item == null)
                return BadRequest("Bad ID");
            this.context.Items.Remove(item);
            await this.context.SaveChangesAsync();
            return Ok(await this.context.Items.ToListAsync());
        }

    }
}
