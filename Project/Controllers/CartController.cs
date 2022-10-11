using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Http;
using Project.Models;

namespace POSAPI.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class CartController : Controller
    {

        private readonly CartContext context;
        public CartController(CartContext context)
        {
            this.context = context;
        }
        [HttpGet]
        public async Task<ActionResult<List<Cart>>> Get()
        {
            return Ok(await this.context.CartItems.ToListAsync());
        }
        [HttpGet("{id}")]
        public async Task<ActionResult<Cart>> Get(int id)
        {
            var item = await this.context.CartItems.FindAsync(id);
            if (item == null)
                return BadRequest("Bad ID");
            return Ok(item);
        }
        [HttpPost]
        public async Task<ActionResult<List<Cart>>> AddItem(Cart addedItem)
        {
            this.context.CartItems.Add(addedItem);
            await this.context.SaveChangesAsync();
            return Ok(await this.context.CartItems.ToListAsync());
        }
        [HttpDelete]
        public async Task<ActionResult<List<Cart>>> DeleteItem(int id)
        {
            var item = await this.context.CartItems.FindAsync(id);
            if (item == null)
                return BadRequest("Bad ID");
            this.context.CartItems.Remove(item);
            await this.context.SaveChangesAsync();
            return Ok(await this.context.CartItems.ToListAsync());
        }
        [HttpPut]
        public async Task<ActionResult<List<Cart>>> UpdateItem(Cart request)
        {
            var cart = await this.context.CartItems.FindAsync(request.Id);
            if (cart == null)
                return BadRequest("Bad ID");
            cart.Name = request.Name;
            cart.Quantity = cart.Quantity + request.Quantity;
            cart.Price = request.Price;
            await this.context.SaveChangesAsync();
            return Ok(await this.context.CartItems.ToListAsync());
        }
    }
}
